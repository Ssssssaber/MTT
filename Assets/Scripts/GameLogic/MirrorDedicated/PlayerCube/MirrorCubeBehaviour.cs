using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class MirrorCubeBehaviour : NetworkBehaviour
{
    private MirrorMovementHandler _movement;

    private void Start()
    {
        _movement = GetComponent<MirrorMovementHandler>();
    }
   
    public void OnRestartButtonPressed(uint cubeCount)
    {
        if (isLocalPlayer) 
        {
            CmdRequestGameRestart(cubeCount);
        }
    }

    [Command]
    private void CmdRequestGameRestart(uint cubeCount)
    {
        MirrorGameManager.instance.RestartTheGame(cubeCount);
    }

    // Called on client input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

        Vector3 dir = context.ReadValue<Vector3>().normalized;
        CmdSetDirection(dir);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

        if (context.performed)
        {
            CmdSetVertical(1);
        }
    }

    [Command]
    private void CmdSetDirection(Vector3 dir)
    {
        _movement.SetDirection(dir);
    }

    [Command]
    private void CmdSetVertical(float vertical)
    {
        _movement.SetVertical(vertical);
    }

    private MirrorColorState _targetedColorState;

    // Server triggers attraction on color state
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<MirrorAttracted>(out var attracted))
        {
            attracted.SetAttractedTo(this.gameObject);
        }

        if (other.gameObject.TryGetComponent<MirrorColorState>(out var colorState))
        {
            colorState.StartAttraction();
        }
    }
}
