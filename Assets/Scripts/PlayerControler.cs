using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControler : NetworkBehaviour
{
    Rigidbody2D rb;
    public float speed;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");
        }
        
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction.normalized * Time.fixedDeltaTime * speed);
    }
}
