using UnityEngine;
using DI;

public class MovementHandler : UpdateableBehaviour
{
    private Rigidbody _rigid;
    [SerializeField]
    private float _speed;
    private Vector3 _direction;

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction; 
    }

    public void SetVertical(float vertical)
    {
        _direction.y = vertical;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public override void SimulationUpdate(float deltaTime)
    {
        if (_direction == Vector3.zero) return;

        _rigid.AddForce(_direction * _speed * Time.deltaTime, ForceMode.VelocityChange);
    }
}
