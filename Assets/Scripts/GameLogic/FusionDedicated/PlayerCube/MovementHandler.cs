using Fusion;
using UnityEngine;

public class MovementHandler : NetworkBehaviour
{
    private Rigidbody _rigid;
    [SerializeField] private float _speed = 50.0f;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
           _rigid.AddForce(data.direction * _speed * Runner.DeltaTime, ForceMode.VelocityChange);
        }
    }
}
