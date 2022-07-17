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
    // Start is called before the first frame update
    void Start()
    {
        SaveDate.PlayerData safeData = SaveMenager.Load<SaveDate.PlayerData>("playerSave.json");
        if (safeData.x == 0f)
        {
            Debug.Log("= 0");
            safeData.x = this.transform.position.x;
            safeData.y = this.transform.position.y;

            SaveMenager.Save(safeData ,"PlayerSave.json");
        }
        Debug.Log(SaveMenager.Load<SaveDate.PlayerData>("playerSave.json").playerName);
        rb = GetComponent<Rigidbody2D>();
        if (isLocalPlayer)
        {
            Camera.cvc.Follow = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = Input.GetAxisRaw("Vertical");
        }
        
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction.normalized * Time.fixedDeltaTime * speed);
    }
}
