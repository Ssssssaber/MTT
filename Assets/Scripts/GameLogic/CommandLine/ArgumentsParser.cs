using UnityEngine;
using System;
using System.Collections.Generic;

public class ArgumentsParser : MonoBehaviour
{
	[Header("Editor arguments")]
	[SerializeField] private string[] _baseCommandLineArguments;
	[SerializeField] InitArguments _arguments = new InitArguments();

	private Dictionary<string, (Type type, string description)> c_helpArguments = new Dictionary<string, (Type, string)>
	{
		{ "--server", (typeof(bool), "Run as a server. Example: --server true") },
		{ "--client", (typeof(bool), "Run as a client. Example: --client true") },
		{ "--recording-time", (typeof(uint), "SERVER: Set the recording time in seconds. Example: --recording-time 60") },
		{ "--player-auto", (typeof(bool), "SERVER: Enable automatic player movement. Example: --player-auto true") },
		{ "--help", (typeof(bool), "Display this help message and exit.") }
	};

	private string[] _rawCommandLineArguments;
	private bool showHelpUI = false;
	private Rect helpWindowRect = new Rect(20, 20, 500, 300);

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

			if (argument == "--help")
			{
				showHelpUI = true;
			}

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
	
	private void OnGUI()
	{
		if (!showHelpUI) return;

		GUILayout.BeginArea(new Rect(20, Screen.height - 220, Screen.width - 40, 200));

		GUILayout.BeginVertical(GUI.skin.box);
		GUILayout.Label("Available Command-Line Arguments:");

		foreach (var arg in c_helpArguments)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label($"{arg.Key}", GUILayout.Width(150));
			GUILayout.Label($"{arg.Value.type.Name}: {arg.Value.description}", GUILayout.ExpandWidth(true));
			GUILayout.EndHorizontal();
		}

		if (GUILayout.Button("Close", GUILayout.Height(30)))
		{
			showHelpUI = false;
		}

		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}