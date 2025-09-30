using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MirrorMovementHandler : NetworkBehaviour
{
    public float _speed = 10f;
    private Rigidbody _rigid;
    private Vector3 _direction;

    // [SyncVar] private Vector3 _syncPosition;
    // [SyncVar] private Vector3 _syncVelocity;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        // if (isServer)
        // {
        //     _syncPosition = transform.position;
        //     _syncVelocity = _rigid.velocity;
        // }
    }

    public void SetDirection(Vector3 direction)
    {
        if (!isServer) return;
        _direction = direction;

        Debug.Log($"Direction set to {_direction}");
    }

    public void SetVertical(float vertical)
    {
        if (!isServer) return;
        _direction.y = vertical;
        Debug.Log($"Direction set to {_direction}");
    }

    [ServerCallback]
    private void Update()
    {
        if (_direction == Vector3.zero) return;
        Debug.Log($"Moving in direction {_direction} with speed {_speed}"); 

        _rigid.AddForce(_direction * _speed * Time.deltaTime, ForceMode.VelocityChange);
        Debug.Log(_rigid.velocity + " <- Velocity" + _rigid.position + " <- Position");
        Debug.Log(transform.position + " <- Position");

        // // Sync position and velocity for clients
        // _syncPosition = transform.position;
        // _syncVelocity = _rigid.velocity;
    }

    // [ClientCallback]
    // private void FixedUpdate()
    // {
    //     if (isServer) return;

    //     // Interpolate position and velocity on client for smoothness
    //     transform.position = Vector3.Lerp(transform.position, _syncPosition, Time.fixedDeltaTime * 10f);
    //     _rigid.velocity = Vector3.Lerp(_rigid.velocity, _syncVelocity, Time.fixedDeltaTime * 10f);
    // }
}
