using System;
using System.Collections;
using System.Data;
// using Mirror;
public interface IClientManager
{
    event Action OnConnected;
    event Action OnDisconnected;
    event Action<ConnectionState, PlayerConnectionStateChangedInfo> OnConnectionStateChanged;
    event Action<string> OnServerMessage;
    //           previous           current
    event Action<ConnectionQuality, ConnectionQuality> OnConnectionQualityChanged;

    void SendMessageToServer(string message);
    bool IsActive();
    IEnumerator Reconnect(float timeout);
    // void ConnectAsGuest();
    void ConnectAuthorized(string credentials); // string == "" - guest
}