using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class MirrorAttracted : NetworkBehaviour
{
    [SyncVar] public GameObject _attractedTo;  // Sync the target object

    public float _strengthOfAttraction = 5.0f;

    private Rigidbody _rigid;
    private Vector3 _distanceVector;


    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void SetAttractedTo(GameObject newAttractedTo)
    {
        if (!isServer) return;
        _attractedTo = newAttractedTo;
    }

    [Server]
    public void PerformAtrraction(float deltaTime)
    {
        if (_attractedTo == null) return;

        // Perform attraction on server
        _distanceVector = _attractedTo.transform.position - transform.position;
        _rigid.AddForce(_strengthOfAttraction * _rigid.mass * _distanceVector.normalized * deltaTime);
    }
}
