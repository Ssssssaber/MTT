
using UnityEngine;
using FishNet;
using FishNet.Transporting;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Init arguments")]
    [SerializeField] bool _initWithCommandLineArguments = true;
    [SerializeField] private InitArguments _initializeArguments;
    private ArgumentsParser _parser;
     public InitArguments GetCommandLineArguments() => _initializeArguments;
    
    // This will now only fire once the network connection is actually established
    public static Action ArgumentsInitialized;

    private bool _eventFired = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _parser = GetComponent<ArgumentsParser>();

        // Subscribe to connection events
        InstanceFinder.ServerManager.OnServerConnectionState += OnServerStateChange;
        InstanceFinder.ClientManager.OnClientConnectionState += OnClientStateChange;
    }

    private void OnDestroy()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.ServerManager.OnServerConnectionState -= OnServerStateChange;
            InstanceFinder.ClientManager.OnClientConnectionState -= OnClientStateChange;
        }
    }

    private void Start()
    {
        _initializeArguments = _parser.GetCommandLineArguments();
        ProcessCommandLineArguments();
        // Removed ArgumentsInitialized?.Invoke() from here
    }

    private void OnServerStateChange(ServerConnectionStateArgs args)
    {
        // Fires when local server starts
        if (args.ConnectionState == LocalConnectionState.Started)
            CheckAndInvokeArguments();
    }

    private void OnClientStateChange(ClientConnectionStateArgs args)
    {
        // Fires when local client starts/connects
        if (args.ConnectionState == LocalConnectionState.Started)
            CheckAndInvokeArguments();
    }

    private void CheckAndInvokeArguments()
    {
        // Ensure we only fire the event once even if Host (which triggers both events)
        if (!_eventFired)
        {
            _eventFired = true;
            Debug.Log("Network Started: Invoking ArgumentsInitialized.");
            ArgumentsInitialized?.Invoke();
        }
    }

    private void ProcessCommandLineArguments()
    {
        if (!_initWithCommandLineArguments) return;

        Transport transport = InstanceFinder.TransportManager.Transport;

        if (_initializeArguments.isClient && _initializeArguments.isServer)
        {
            Debug.Log("Starting Host...");
            SetPort(transport, _initializeArguments.serverPort);
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        }
        else if (_initializeArguments.isClient)
        {
            Debug.Log($"Starting Client connecting to {_initializeArguments.serverAddress}...");
            SetAddress(transport, _initializeArguments.serverAddress);
            SetPort(transport, _initializeArguments.serverPort);
            InstanceFinder.ClientManager.StartConnection();
        }
        else if (_initializeArguments.isServer)
        {
            Debug.Log($"Starting Server on port {_initializeArguments.serverPort}...");
            SetPort(transport, _initializeArguments.serverPort);
            InstanceFinder.ServerManager.StartConnection();
        }
    }

    private void SetPort(Transport transport, ushort port)
    {
        if (transport is FishNet.Transporting.Tugboat.Tugboat tugboat)
            tugboat.SetPort(port);
    }

    private void SetAddress(Transport transport, string address)
    {
        if (transport is FishNet.Transporting.Tugboat.Tugboat tugboat)
            tugboat.SetClientAddress(address);
    }
}
