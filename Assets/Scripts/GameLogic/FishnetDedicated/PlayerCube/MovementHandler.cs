using UnityEngine;
using FishNet.Object;

public class MovementHandler : NetworkBehaviour
{
    [SerializeField] bool _clientPhysics = true;
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
        if (!_clientPhysics) DisableClientPhysics();
    }

    private void DisableClientPhysics()
    {
        if (IsClientOnlyInitialized)
        {
            _rigid.isKinematic = true;
            var collider = GetComponent<BoxCollider>();
            collider.enabled = false; 
        }

    }

    private void Update()
    {
        if (!IsServerInitialized) return;
        if (_direction == Vector3.zero) return;

        _rigid.AddForce(_direction * _speed * Time.deltaTime, ForceMode.VelocityChange);
    }
}
