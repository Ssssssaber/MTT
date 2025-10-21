using Fusion;
using System.Collections;
using UnityEngine;

public class CubeBehaviour : NetworkBehaviour
{
    private NetworkObject _networkObject;

    private void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    // Trigger on state authority
    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent<Attracted>(out var attracted))
        {
            attracted.SetAttractedTo(Object);  // NetworkObject reference
        }

        if (other.TryGetComponent<ColorState>(out var colorState))
        {
            colorState.StartAttraction();
        }
    }
}
