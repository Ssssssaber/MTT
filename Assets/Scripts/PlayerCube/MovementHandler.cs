using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    private Rigidbody _rigid;
    [SerializeField]
    private float _speed;

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void Move(Vector3 direction)
    {
        _rigid.AddForce(direction * _speed, ForceMode.VelocityChange);
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }
}
