[System.Serializable]
public class InitArguments
{
	public string ClientId = "Client0";
	public string serverAddress = "127.0.0.1";
	public ushort serverPort = 5674;
	public bool playerAuto = false;
	public bool isClient = false;
	public bool isServer = false;
	public uint recordingTime = 0;
	public uint cubeCount = 500;
}
