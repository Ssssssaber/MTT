using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeBehaviour : MonoBehaviour
{
    private MovementHandler _movement;
    public void Move(InputAction.CallbackContext context)
    {
        _movement.Move(context.ReadValue<Vector3>().normalized);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movement.Move(Vector3.up);
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        _movement = GetComponent<MovementHandler>();
    }
}
