using UnityEngine;

public class ArgumentParser : MonoBehaviour
{
	[Header("Editor arguments")]
	[SerializeField] private string[] _baseCommandLineArguments;
	[SerializeField] private bool _useEditorCommandLineArguments = false;
	
	[SerializeField] CommandLineArguments _arguments = new CommandLineArguments();

	private string[] _rawCommandLineArguments;

	public CommandLineArguments GetCommandLineArguments()
	{
		return _arguments;
	}

	private void Awake()
	{
	#if UNITY_EDITOR
		if (!_useEditorCommandLineArguments) return;
	#endif

		_rawCommandLineArguments = GetArguments();
		Parse(_rawCommandLineArguments);

		Debug.Log($"Resulting command line arguments: " + 
					$"{_arguments.isServer}, {_arguments.isClient}, {_arguments.recordingTime}");
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
			}
		}
	}
}