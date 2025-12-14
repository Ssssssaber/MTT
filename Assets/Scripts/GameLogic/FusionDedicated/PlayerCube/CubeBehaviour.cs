
using Fusion;
using System.Collections;
using System.Globalization;
using UnityEngine;

public class CubeBehaviour : NetworkBehaviour
{
    private NetworkObject _networkObject;

    private void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
    }

    private void Start()
    {
        SetAsCurrent();
    }
    private void SetAsCurrent()
    {
        if (!HasInputAuthority) return;

        GameManager.Instance.SetCurrentPlayerCube(this);
    }

    public void AskForRestart(uint cubeCount)
    {
        if (HasStateAuthority)
            GameManager.Instance.RestartTheGame(cubeCount);
        else
            RpcAskForRestartGame(cubeCount);
    }

	[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcAskForRestartGame(uint cubeCount)
    {
        GameManager.Instance.RestartTheGame(cubeCount);
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
