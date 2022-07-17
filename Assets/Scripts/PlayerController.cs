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

    void positionLoad()
    {
        if (safeData.x == 0f)
        {
            safeData.x = this.transform.position.x;
            safeData.y = this.transform.position.y;
            SaveMenager.Save(safeData, "PlayerSave.json");
        }
        else
        {
            transform.position = new Vector2(safeData.x, safeData.y);
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
        positionLoad();
        cameraConnect();
        StartCoroutine(SavePosition());
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

    IEnumerator SavePosition()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(10f);
            safeData.x = transform.position.x;
            safeData.y = transform.position.y;
            SaveMenager.Save(safeData, "PlayerSave.json");
            Debug.Log("saved" + safeData.x + safeData.y);
        }
    }
}
