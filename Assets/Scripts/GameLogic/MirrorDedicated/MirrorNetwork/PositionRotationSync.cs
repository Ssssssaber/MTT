using Mirror;
using UnityEngine;

public class PositionRotationSync : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 syncedPosition;

    [SyncVar(hook = nameof(OnRotationChanged))]
    private Quaternion syncedRotation;

    [SerializeField] private float lerpSpeed = 10f;

    private void Start()
    {
        // Initialize sync vars on server
        if (isServer)
        {
            syncedPosition = transform.position;
            syncedRotation = transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            syncedPosition = transform.position;
            syncedRotation = transform.rotation;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, syncedPosition, Time.fixedDeltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, syncedRotation, Time.fixedDeltaTime * lerpSpeed);
        }
    }

    private void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        // Optionally, do something when position changes
    }

    private void OnRotationChanged(Quaternion oldRot, Quaternion newRot)
    {
        // Optionally, do something when rotation changes
    }
}