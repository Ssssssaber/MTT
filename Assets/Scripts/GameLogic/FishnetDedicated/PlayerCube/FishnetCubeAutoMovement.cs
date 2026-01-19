using FishNet.Object;
using UnityEngine;

public class FishNetCubeAutoMovement : NetworkBehaviour
{
    [SerializeField] private float _moveInterval = 0.1f;
    private MovementHandler _movement;
    private Vector3 _currentDirection = Vector3.zero;
    
    private float _moveTimer = 0.0f;

    private void Awake()
    {

        _movement = GetComponent<MovementHandler>();
    }

    private void Start()
    {
        _moveTimer = _moveInterval;
    }

    private void Update()
    {
        if (!IsOwner) return;

        _moveTimer -= Time.deltaTime;
        if (_moveTimer < 0f)
        {
            _moveTimer = _moveInterval;
            UpdateMovementDirection();
        }
    }

    private void UpdateMovementDirection()
    {
        float x = Mathf.Sin(Time.time);
        float z = Mathf.Cos(Time.time);

        _currentDirection = new Vector3(x, 0, z).normalized;
        OnMove(_currentDirection);
        Debug.Log($"move dir{_currentDirection}");
    }

    private void OnMove(Vector3 direction)
    {
        if (!IsOwner) return;

        ServerSetDirection(direction);
    }

    private void ServerSetDirection(Vector3 dir)
    {
        _movement.SetDirection(dir);
    }

    private void ServerSetVertical(float vertical)
    {
        _movement.SetVertical(vertical);
    }
}
