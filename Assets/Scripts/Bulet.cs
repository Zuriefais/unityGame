using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bulet :  NetworkBehaviour
{
    uint owner;
    Vector2 target;
    bool inited = false;

    [Server]
    public void Init(uint owner, Vector2 target)
    {
        this.owner = owner;
        this.target = target;
        inited = true;
    }

    private void Update()
    {
        if (inited && isServer)
        {
            transform.Translate((target - new Vector2(transform.position.x, transform.position.y)).normalized * 0.10f);
            if (transform.position == new Vector3(target.x ,target.y))
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
