using Mirror;
public class ConnectionInfo
{
    public static bool IsSafeToAskForConnectionInfo()
    {
        if (!NetworkServer.active && !NetworkClient.active) return false;
        
        // Ensure client is fully connected and has a valid connection object
        if (NetworkClient.active && (NetworkClient.connection == null || !NetworkClient.isConnected))
        {
            return false;
        }

        return true;
    }

    public static string GetConnectionInfo()
    {
        // 1. Check if we are the Server/Host
        if (NetworkServer.active)
        {
            return NetworkClient.active ? "Host" : "Server";
        }

        return "Client";
    }
}
