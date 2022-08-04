using UnityEngine;
using Mirror;
using Cinemachine;

public class PlayerController : NetworkBehaviour
{
    public float speed;
    public GameObject mainCamera;
    private Rigidbody2D _rigidBody;
    private Vector2 _direction;
    private CinemachineVirtualCamera _cinemachineCamera;
    private SaveDate.PlayerData _safeData = SaveMenager.Load<SaveDate.PlayerData>("playerSave.json");

    private void CameraConnect()
    {
        if (isLocalPlayer)
        {
            Camera.cvc.Follow = gameObject.transform;
        }
    }

    private void CheckInput()
    {
        if (isLocalPlayer)
        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.y = Input.GetAxisRaw("Vertical");
        }
    }

    private void MovePosition()
    {
        _rigidBody.MovePosition(_rigidBody.position + _direction.normalized * Time.fixedDeltaTime * speed);
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        CameraConnect();     
    }

    private void Update()
    {
        CheckInput();
    }

    void FixedUpdate()
    {
        MovePosition();
    }
}
