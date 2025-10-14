public enum NetworkMode
{
    MessageOnly,
    SceneShared
}
public enum UserType
{
    Authorized,
    Guest
}
public enum StatusAuthorized
{
    Success,
    Failed
}

public enum StatusDisconnection
{
    BadConnection,
    UserRequest,
    Timeout
}

public enum ConnectionQuality : byte
{
    EXCELLENT,  // ideal experience for high level competitors
    GOOD,       // very playable for everyone but high level competitors
    FAIR,       // very noticeable latency, not very enjoyable anymore
    POOR,       // unplayable
    ESTIMATING, // still estimating
}

public enum ConnectionState
{
    Disconnected,
    Connecting,
    Authenticating,
    Connected,
    Reconnecting
}
public struct PlayerConnectionStateChangedInfo
{
    public ConnectionQuality PreviousQuality;
    public string message;
}

// cold be relocated to voice chat intefrace 
public enum VoiceMode
{
    AllInRange,
    AuthLevelInRange,
    PrivateRoom
}

