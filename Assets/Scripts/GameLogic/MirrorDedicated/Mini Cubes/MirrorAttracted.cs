using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class MirrorAttracted : NetworkBehaviour
{
    [SerializeField] private bool _clientPhysics = false;
    [SyncVar] public GameObject _attractedTo;  // Sync the target object

    public float _strengthOfAttraction = 5.0f;

    private Rigidbody _rigid;
    private Vector3 _distanceVector;


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
