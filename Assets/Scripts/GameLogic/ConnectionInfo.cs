using FishNet;
using FishNet.Transporting;
using UnityEngine;

public class ConnectionInfo
{
    public static string GetConnectionInfo()
    {
        if (InstanceFinder.ServerManager.Started)
        {
            return "Server";
        }

        return "Client";
    }
}