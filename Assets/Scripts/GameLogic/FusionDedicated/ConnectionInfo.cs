using System.Linq;
using Fusion;

public class ConnectionInfo
{
    public static string GetConnectionInfo()
    {
        var runner = NetworkRunner.Instances.FirstOrDefault<NetworkRunner>();

        if (runner == null) return "Offline";

        if (runner.IsServer)
        {
            return "Server";
        }
        
        if (runner.IsClient)
        {
            return "Client";
        }

        return "Offline";
    }
}