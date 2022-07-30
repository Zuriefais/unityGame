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
    private SaveDate.PlayerData safeData = SaveMenager.Load<SaveDate.PlayerData>("playerSave.json");

    void cameraConnect()
    {
        if (isLocalPlayer)
        {
            Camera.cvc.Follow = gameObject.transform;
        }
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
        rb = GetComponent<Rigidbody2D>();
        cameraConnect();     
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
