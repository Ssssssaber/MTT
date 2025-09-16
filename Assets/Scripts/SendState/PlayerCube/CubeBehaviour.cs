using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using SendState.MiniCubes;
using SendState.MirrorNetwork;
using SendState.DI;

namespace SendState.PlayerCube
{
    [RequireComponent(typeof(MovementHandler))]
    public class CubeBehaviour : UpdateableBehaviour
    {
        private MovementHandler _movement;
        public void Move(InputAction.CallbackContext context)
        {
            _movement.SetDirection(context.ReadValue<Vector3>().normalized);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _movement.SetVertical(1);
            }
        }
        // Start is called before the first frame update
        private void Start()
        {
            _movement = GetComponent<MovementHandler>();
        }

        private ColorState _targetedColorState;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out _targetedColorState))
            {
                _targetedColorState.StartAttraction();
            }
        }

        public override ObjectRepresentation GetRepresentation()
        {
            return ObjectRepresentation.PlayerCube;
        }
    }
}