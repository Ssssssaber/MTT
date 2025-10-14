using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public static IClientManager Client { get; private set; }
    public static IServerManager Server { get; private set; }

    [SerializeField] private GameObject _serverGO;
    [SerializeField] private GameObject _clientGO;

    public static bool IsServer()
    {
        if (Server == null) return false;
        else return Server.IsActive();
    }
    public static bool IsClient()
    {
        if (Client == null) return false;
        else return Client.IsActive();
    }

    void Awake()
    {
        if (_serverGO != null) Server = _serverGO.GetComponent<IServerManager>();
        if (_clientGO != null) Client = _clientGO.GetComponent<IClientManager>();
    }
}