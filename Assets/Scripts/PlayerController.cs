using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Mirror;
using Cinemachine;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    public GameObject mainCamera;
    [SyncVar]
    public float health;
    public float maxHealth = 2f;
    public TextMeshProUGUI healthUI;
    private bool _canControll = true;
    private Rigidbody2D _rigidBody;
    private Vector2 _direction;

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
                RemoveHealth(1f);
            }
        }
    }

    private void MovePosition()
    {
        _rigidBody.MovePosition(_rigidBody.position + _direction.normalized * Time.fixedDeltaTime * speed);
    }

    private void RemoveHealth(float damage)
    {
        health = health - damage;
        healthUI.text = "Здоровье = " + health;
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

    [Command]
    private void Respawn(NetworkConnectionToClient sender = null)
    {
        string senderName = sender.identity.name;
        PlayerController senderController = sender.identity.GetComponent<PlayerController>();
        senderController._canControll = false;
        List<SaveDate.PlayersPositions> playersPositions = SaveMenager.Load<List<SaveDate.PlayersPositions>>("playersPositions.json");
        foreach (var playerPosition in playersPositions)
        {
            if (playerPosition.playerName == senderName)
            {
                Vector2 newPossition = new(playerPosition.playerPosition.x, playerPosition.playerPosition.y);
                sender.identity.transform.position = newPossition;
                senderController.health = senderController.maxHealth;
                senderController._canControll = true;
                return;
            }
        }
    }

    private void Start()
    {
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
