using System;
using System.Collections;
using Unity.VisualScripting;

public interface IServerManager
{
    event Action OnServerStarted;
    event Action OnServerStopped;
    event Action OnClientConnected;
    event Action OnClientDisconnected;
    event Action<string> OnClientMessage;

    bool IsActive();
    void SendMessageToClients(string message, bool toReady = false);
    bool AuthorizeClient(string credentials); 
}
