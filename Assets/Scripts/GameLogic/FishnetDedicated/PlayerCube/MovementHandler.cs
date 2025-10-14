using UnityEngine;
using FishNet.Object;

public class MovementHandler : NetworkBehaviour
{
    private Rigidbody _rigid;
    public float _speed;
    private Vector3 _direction;

    [ServerRpc]
    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    [ServerRpc]
    public void SetDirection(Vector3 direction)
    {
        _direction = direction; 
    }

    [ServerRpc]
    public void SetVertical(float vertical)
    {
        _direction.y = vertical;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsServerInitialized) return;
        if (_direction == Vector3.zero) return;

        _rigid.AddForce(_direction * _speed * Time.deltaTime, ForceMode.VelocityChange);
    }
}
