using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private Camera _camera;
    [SerializeField]
    private Vector3 _offset = new Vector3(10, 10, 10);
    [SerializeField]
    private Transform _target;

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void FollowTarget()
    {
        Vector3 newPosition = _target.position;
        newPosition += _offset;
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, newPosition, 0.05f);
        _camera.transform.LookAt(_target);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_target) FollowTarget();
    }
}
