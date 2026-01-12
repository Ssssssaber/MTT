using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("Init arguments")]
	[SerializeField] bool _initWithCommandLineArguments = true;
	[SerializeField] private InitArguments _initializeArguments;
	private ArgumentsParser _parser;

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
	}

	private void ProcessCommandLineArguments()
	{
		if (!_initWithCommandLineArguments) return;

		if (_initializeArguments.isClient)
		{
			NetworkManager.singleton.StartClient();
		}
		else if (_initializeArguments.isServer)
		{
			NetworkManager.singleton.StartServer();
		}
		else if (_initializeArguments.isClient && _initializeArguments.isServer)
		{
			NetworkManager.singleton.StartHost();
		}

		Debug.Log($"Rec time is: {_initializeArguments.recordingTime}");
	}
}