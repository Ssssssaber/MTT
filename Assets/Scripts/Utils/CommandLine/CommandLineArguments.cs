[System.Serializable]
public class CommandLineArguments
{
	public bool isClient;
	public bool isServer;
	public uint recordingTime;
	
	public CommandLineArguments(bool isClient = true, bool isServer = false, uint recordingTime = 100)
	{
		this.isClient = isClient;
		this.isServer = isServer;
		this.recordingTime = recordingTime;
	}
}

