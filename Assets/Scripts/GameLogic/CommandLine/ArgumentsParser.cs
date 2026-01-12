using UnityEngine;

public class ArgumentsParser : MonoBehaviour
{
	[Header("Editor arguments")]
	[SerializeField] private string[] _baseCommandLineArguments;
	
	[SerializeField] InitArguments _arguments = new InitArguments();

	private string[] _rawCommandLineArguments;

	public InitArguments GetCommandLineArguments()
	{
		return _arguments;
	}

	private void Awake()
	{
		_rawCommandLineArguments = GetArguments();
		Parse(_rawCommandLineArguments);
	}

	private string[] GetArguments()
	{
	#if UNITY_EDITOR
		return _baseCommandLineArguments;
	#else
		return Environment.GetCommandLineArgs();
	#endif
	}

	private void Parse(string[] arguments)
	{
		for (int i = 0; i < arguments.Length; i++)
		{
			string argument = arguments[i].ToLower();

			switch(argument)
			{
				case "--server":
					if (i + 1 < arguments.Length) _arguments.isServer = bool.Parse(arguments[i + 1]);
					break;
				case "--client":
					if (i + 1 < arguments.Length) _arguments.isClient = bool.Parse(arguments[i + 1]);
					break;
				case "--recording-time":
					if (i + 1 < arguments.Length) _arguments.recordingTime = uint.Parse(arguments[i + 1]);
					break;
				case "--player-auto":
					if (i + 1 < arguments.Length) _arguments.playerAuto = bool.Parse(arguments[i + 1]);
					break;
			}
		}
	}
}