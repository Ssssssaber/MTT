using System.Collections;
using System.Collections.Generic;
using DI;
using UnityEngine;

namespace SendState.MiniCubes
{
    [RequireComponent(typeof(Rigidbody))]
    class Attracted : UpdateableBehaviour
    {
        public GameObject _attractedTo;

        [SerializeField]
        public float _strengthOfAttraction = 5.0f;

        private Rigidbody _rigid;
        private Vector3 _distanceVector;

        public void SetAttractedTo(GameObject newAttractedTo)
        {
            _attractedTo = newAttractedTo;
        }

        private void Start()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        public void PerformAtrraction(float deltaTime)
        {
            // if (_attractedTo == null) return;

            _distanceVector = _attractedTo.transform.position - transform.position;
            _rigid.AddForce(_strengthOfAttraction * _rigid.mass * _distanceVector.normalized * deltaTime);
        }
    }
}
