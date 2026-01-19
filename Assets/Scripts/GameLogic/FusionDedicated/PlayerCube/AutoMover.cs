using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class AutoMover : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private float _moveInterval = 0.1f;
    private Vector3 _currentDirection = Vector3.zero;
    private float _moveTimer = 0.0f;
    private NetworkRunner _runner;

    private void Awake()
    {
        _moveTimer = _moveInterval;
        _runner = FindObjectOfType<NetworkRunner>();
        if (_runner != null)
        {
            _runner.AddCallbacks(this);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority) return;

        _moveTimer -= Runner.DeltaTime;
        if (_moveTimer < 0f)
        {
            _moveTimer = _moveInterval;
            UpdateMovementDirection();
        }
    }

    private void UpdateMovementDirection()
    {
        float x = Mathf.Sin(Runner.SimulationTime);
        float z = Mathf.Cos(Runner.SimulationTime);

        _currentDirection = new Vector3(x, 0, z).normalized;
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!Object.HasInputAuthority) return;

        var data = new NetworkInputData();
        data.direction = _currentDirection;
        input.Set(data);
    }

    // Required INetworkRunnerCallbacks methods
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {}
}
