using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class Camera : MonoBehaviour
{
    public static CinemachineVirtualCamera cvc;
    void Start()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
        Debug.Log(cvc, cvc.transform);
    }
}
