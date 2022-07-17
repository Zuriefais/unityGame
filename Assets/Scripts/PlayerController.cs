using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class PlayerController : NetworkBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public GameObject mainCamera;
    private Vector2 direction;
    private CinemachineVirtualCamera cinemachineCamera;

    void cameraConnect()
    {
        if (isLocalPlayer)
        {
            Camera.cvc.Follow = gameObject.transform;
        }
    }

    void positionUpdate()
    {
        SaveDate.PlayerData safeData = SaveMenager.Load<SaveDate.PlayerData>("playerSave.json");
        if (safeData.x == 0f)
        {
            Debug.Log("= 0");
            safeData.x = this.transform.position.x;
            safeData.y = this.transform.position.y;

            SaveMenager.Save(safeData, "PlayerSave.json");
        }
        Debug.Log(SaveMenager.Load<SaveDate.PlayerData>("playerSave.json").playerName);
    }

    void CheckInput()
    {
        if (isLocalPlayer)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = Input.GetAxisRaw("Vertical");
        }
    }

    void MovePosition()
    {
        rb.MovePosition(rb.position + direction.normalized * Time.fixedDeltaTime * speed);
    }

    void Start()
    {
        positionUpdate();
        cameraConnect();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckInput();
    }

    void FixedUpdate()
    {
        MovePosition();
    }
}
