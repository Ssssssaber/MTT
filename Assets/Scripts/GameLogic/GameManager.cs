using UnityEngine;
using Mirror;
using System;
using kcp2k;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("Init arguments")]
	[SerializeField] bool _initWithCommandLineArguments = true;
	[SerializeField] private InitArguments _initializeArguments;
	private ArgumentsParser _parser;
	public static Action ArgumentsInitialized;

	public InitArguments GetCommandLineArguments()
	{
		return _initializeArguments;
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		_parser = GetComponent<ArgumentsParser>();
	}

	private void Start()
	{
		_initializeArguments = _parser.GetCommandLineArguments();
		ProcessCommandLineArguments();
		ArgumentsInitialized?.Invoke();
	}

	private void ProcessCommandLineArguments()
	{
		if (!_initWithCommandLineArguments) return;

		if (_initializeArguments.isClient && _initializeArguments.isServer)
		{
			Debug.Log("starting host");
			SetPort(_initializeArguments.serverPort);
			NetworkManager.singleton.StartHost();
		}
		else if (_initializeArguments.isClient)
		{
			Debug.Log("starting cient");
			NetworkManager manager = NetworkManager.singleton;
			manager.networkAddress = _initializeArguments.serverAddress;
			SetPort(_initializeArguments.serverPort);
			manager.StartClient();
		}
		else if (_initializeArguments.isServer)
		{
			Debug.Log("starting server");
			SetPort(_initializeArguments.serverPort);
			NetworkManager.singleton.StartServer();
		}

		Debug.Log($"Rec time is: {_initializeArguments.recordingTime}");
	}

	private void SetPort(ushort port)
	{
		// Most Mirror transports use a 'Port' or 'port' field
		if (Transport.active is KcpTransport kcp)
		{
			kcp.Port = port;
		}
		else if (Transport.active is TelepathyTransport telepathy)
		{
			telepathy.port = port;
		}
	}
}