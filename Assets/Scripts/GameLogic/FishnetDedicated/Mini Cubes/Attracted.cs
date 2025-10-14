using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

[RequireComponent(typeof(Rigidbody))]
class Attracted : NetworkBehaviour
{
    private readonly SyncVar<GameObject> _attractedTo = new SyncVar<GameObject>();

    public float _strengthOfAttraction = 5.0f;

    private Rigidbody _rigid;
    private Vector3 _distanceVector;

    [Server]
    public void SetAttractedTo(GameObject newAttractedTo)
    {
        if (!IsServerInitialized) return;

        _attractedTo.Value = newAttractedTo;
    }

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    [Server]
    public void PerformAtrraction(float deltaTime)
    {
        if (_attractedTo == null) return;

        _distanceVector = _attractedTo.Value.transform.position - transform.position;
        _rigid.AddForce(_strengthOfAttraction * _rigid.mass * _distanceVector.normalized * deltaTime);
    }
}

