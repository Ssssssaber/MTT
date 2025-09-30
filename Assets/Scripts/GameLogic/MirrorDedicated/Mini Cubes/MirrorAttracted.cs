using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class MirrorAttracted : NetworkBehaviour
{
    [SyncVar] public GameObject _attractedTo;  // Sync the target object

    public float _strengthOfAttraction = 5.0f;

    private Rigidbody _rigid;
    private Vector3 _distanceVector;

    [SyncVar] private Vector3 _syncPosition;
    [SyncVar] private Vector3 _syncVelocity;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        if (isServer)
        {
            _syncPosition = transform.position;
            _syncVelocity = _rigid.velocity;
        }
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

        // Sync position and velocity
        _syncPosition = transform.position;
        _syncVelocity = _rigid.velocity;
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (isServer) return;

        // Interpolate on client
        transform.position = Vector3.Lerp(transform.position, _syncPosition, Time.fixedDeltaTime * 10f);
        _rigid.velocity = Vector3.Lerp(_rigid.velocity, _syncVelocity, Time.fixedDeltaTime * 10f);
    }
}
