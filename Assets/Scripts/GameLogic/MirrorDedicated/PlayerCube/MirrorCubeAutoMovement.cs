using Mirror;
using UnityEngine;

public class MirrorCubeAutoMovement : NetworkBehaviour
{
	[SerializeField] private float _moveInterval = 0.1f;
	private MirrorMovementHandler _movement;
	private Vector3 _currentDirection = Vector3.zero;
	
	private float _moveTimer = 0.0f;
	private void Start()
	{
		_movement = GetComponent<MirrorMovementHandler>();
		_moveTimer = _moveInterval;
	}

	private void Update()
	{
		if (!isLocalPlayer) return;

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
	}

	private void OnMove(Vector3 direction)
	{
		if (!isLocalPlayer) return;

		CmdSetDirection(direction);
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
}