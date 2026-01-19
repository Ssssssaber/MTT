using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Connection;

public class CubeBehaviour : NetworkBehaviour
{
    private MovementHandler _movement;

    [TargetRpc]
    public void TargetRpcSetAsCurrentPlayer(NetworkConnection conn)
    {
        FishNetGameManager.Instance.SetCurrentPlayerCube(this);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        _movement.SetDirection(context.ReadValue<Vector3>().normalized);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            _movement.SetVertical(1);
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        _movement = GetComponent<MovementHandler>();
        SetAsCurrent();
    }
    private void SetAsCurrent()
    {
        if (!IsOwner) return;

         FishNetGameManager.Instance.SetCurrentPlayerCube(this);
    }

    [ServerRpc]
    public void ServerRpcAskForRestartGame(uint cubeCount)
    {
        FishNetGameManager.Instance.RestartTheGame(cubeCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServerInitialized) return;

        if (other.gameObject.TryGetComponent<Attracted>(out var attracted))
        {
            attracted.SetAttractedTo(this.gameObject);
        }

        if (other.gameObject.TryGetComponent<ColorState>(out var colorState))
        {
            colorState.StartAttraction();
        }
    }
}
