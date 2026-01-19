using UnityEngine;
using System;
using Fusion;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("Fusion NetworkManager")]
    public BasicSpawner _spawner;
    public NetworkRunner Runner {get; private set; }

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
        Runner = _spawner.GetComponent<NetworkRunner>();
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
			_spawner.StartGame(Fusion.GameMode.Host, _initializeArguments.serverAddress, _initializeArguments.serverPort);
		}
		else if (_initializeArguments.isClient)
		{
			Debug.Log("starting cient");
			_spawner.StartGame(Fusion.GameMode.Client
            , _initializeArguments.serverAddress, _initializeArguments.serverPort);
		}
		else if (_initializeArguments.isServer)
		{
			Debug.Log("starting server");
			_spawner.StartGame(Fusion.GameMode.Server, _initializeArguments.serverAddress, _initializeArguments.serverPort);
		}

		Debug.Log($"Rec time is: {_initializeArguments.recordingTime}");
	}
}