using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeBehaviour : MonoBehaviour
{
    private MovementHandler _movement;
    private Vector3 _direction;

    public void Move(InputAction.CallbackContext context)
    {
        Debug.Log("Move " + context.ReadValue<Vector3>());

        Vector3 input = context.ReadValue<Vector3>();
        _movement.Move(input.normalized);
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

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {

        // _movement.Move()
    }
}
