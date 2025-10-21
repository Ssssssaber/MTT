using UnityEngine;
using Fusion;

[RequireComponent(typeof(Rigidbody))]
class Attracted : NetworkBehaviour
{
    public NetworkObject _attractedTo;

    public float _strengthOfAttraction = 5.0f;

    private Rigidbody _rigid;
    private Vector3 _distanceVector;

    public void SetAttractedTo(NetworkObject newAttractedTo)
    {
        _attractedTo = newAttractedTo;
    }

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void PerformAtrraction(float deltaTime)
    {
         if (_attractedTo == null) return;

        _distanceVector = _attractedTo.transform.position - transform.position;
        _rigid.AddForce(_strengthOfAttraction * _rigid.mass * _distanceVector.normalized * deltaTime);
    }
}

