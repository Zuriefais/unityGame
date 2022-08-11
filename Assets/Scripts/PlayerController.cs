using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mirror;
using Cinemachine;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    [SyncVar]
    public string playerName;
    public float speed;
    public GameObject mainCamera;
    [SyncVar(hook = nameof(OnHealhChange))]
    public float health;
    [SyncVar]
    public float maxHealth = 100f;
    public TextMeshProUGUI playerNameUI;
    public TextMeshProUGUI healthUI;
    [SyncVar]
    private bool _canControll = true;
    private Rigidbody2D _rigidBody;
    private Vector2 _direction;
    public GameObject bulletPrefab;

    private void CameraConnect()
    {
        if (isLocalPlayer)
        {
            Camera.cvc.Follow = gameObject.transform;
        }
    }

    private void CheckInput()
    {
        if (isLocalPlayer && _canControll)
        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.y = Input.GetAxisRaw("Vertical");
            if (Input.GetButtonDown("Fire1"))
            {
                SpawnBullet(netId ,Input.mousePosition);
            }
        }
    }

    [Command]
    private void SpawnBullet(uint owner ,Vector2 target,NetworkConnectionToClient sender = null)
    {
        GameObject bulletGo = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(bulletGo);
        bulletGo.GetComponent<Bulet>().Init(owner, target);
    }

    private void MovePosition()
    {
        _rigidBody.MovePosition(_rigidBody.position + _direction.normalized * Time.fixedDeltaTime * speed);
    }

    [Command]
    private void RemoveHealth(float damage, NetworkConnectionToClient sender = null)
    {
        sender.identity.GetComponent<PlayerController>().health = Math.Max(health - damage, 0);
    }

    private void OnHealhChange(float oldHelth, float newHealth)
    {
        oldHelth = newHealth;
        Debug.Log(health);
        if (health <= 0)
        {
            Respawn();
        }
        healthUI.text = "Здоровье = " + health;
    }

    [Client]
    private void ChangeCamera()
    {
        Camera.cvc.Follow = GameObject.Find("Dog").transform;
    }

    [Command(requiresAuthority = false)]
    private void Respawn(NetworkConnectionToClient sender = null)
    {
        PlayerController senderController = sender.identity.GetComponent<PlayerController>();
        senderController._canControll = false;
        List<SaveDate.PlayersPositions> playersPositions = SaveMenager.Load<List<SaveDate.PlayersPositions>>("playersPositions.json");
        foreach (var playerPosition in playersPositions)
        {
            if (playerPosition.playerName == senderController.playerName)
            {
                teleport(sender, playerPosition.playerPosition);
                senderController.health = senderController.maxHealth;
                senderController._canControll = true;
                return;
            }
            else
            {
                Debug.Log(playerPosition.playerName == senderController.playerName);
            }
        }
        senderController.health = senderController.maxHealth;
    }

    [TargetRpc]
    private void teleport(NetworkConnection target, Vector2 teleportPosition)
    {
        transform.position = teleportPosition;
    }

    [Command(requiresAuthority = false)]
    private void activateAuthority(NetworkConnectionToClient sender = null)
    {
        sender.identity.AssignClientAuthority(sender);
    }

    private void Start()
    {
        Debug.Log(hasAuthority);
        if (isLocalPlayer)
        {
            playerName = SaveMenager.Load<SaveDate.PlayerData>("playerSave.json").playerName;
        }
        playerNameUI.text = playerName;
        if(!hasAuthority)
        {
            activateAuthority();
        }
        Debug.Log(playerName);
        _rigidBody = GetComponent<Rigidbody2D>();
        var newhealthUIs = FindObjectsOfType<TextMeshProUGUI>();
        for(var i = 0; i < newhealthUIs.Length; i++)
        {
            if(newhealthUIs[i].name == "Health")
            {
                healthUI = newhealthUIs[i];
            }
        }
        health = maxHealth;
        healthUI.text = "Здоровье = " + health;
        CameraConnect();     
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        MovePosition();
    }
}
