using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PositionRotationSync : NetworkBehaviour
{
    private readonly SyncVar<Vector3> syncedPosition = new SyncVar<Vector3>();
    private readonly SyncVar<Quaternion> syncedRotation = new SyncVar<Quaternion>();

    [SerializeField] private float lerpSpeed = 10f;

    private void Awake()
    {
        // Set up hooks for sync var changes
        syncedPosition.OnChange += OnPositionChanged;
        syncedRotation.OnChange += OnRotationChanged;
    }

    private void Start()
    {
        // Initialize sync vars on server
        if (IsServer)
        {
            syncedPosition.Value = transform.position;
            syncedRotation.Value = transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            syncedPosition.Value = transform.position;
            syncedRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, syncedPosition.Value, Time.fixedDeltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, syncedRotation.Value, Time.fixedDeltaTime * lerpSpeed);
        }
    }

    private void OnPositionChanged(Vector3 prev, Vector3 next, bool asServer)
    {
        // Optionally, do something when position changes
        // 'asServer' indicates if the change occurred on the server
    }

    private void OnRotationChanged(Quaternion prev, Quaternion next, bool asServer)
    {
        // Optionally, do something when rotation changes
        // 'asServer' indicates if the change occurred on the server
    }

    public override void OnStopNetwork()
    {
        // Clean up hooks when the object is despawned or network stops
        syncedPosition.OnChange -= OnPositionChanged;
        syncedRotation.OnChange -= OnRotationChanged;
        base.OnStopNetwork();
    }
}

