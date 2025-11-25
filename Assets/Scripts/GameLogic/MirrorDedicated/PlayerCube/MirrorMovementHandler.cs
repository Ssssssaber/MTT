using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MirrorMovementHandler : NetworkBehaviour
{
    [SerializeField] private bool _clientPhysics = false;
    public float _speed = 10f;
    private Rigidbody _rigid;
    private Vector3 _direction;
    
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        if (!_clientPhysics) DisableClientPhysics();
    }

    private void DisableClientPhysics()
    {
        if (isClientOnly)
        {
            _rigid.isKinematic = true;
            var collider = GetComponent<BoxCollider>();
            collider.enabled = false; 
        }

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
    private void FixedUpdate()
    {
        if (_direction == Vector3.zero) return;

        _rigid.AddForce(_direction * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
