#if UNITY_EDITOR

using Unity.Profiling;

public static class GameStatistics
{
    public static readonly ProfilerCategory NetworkCategory = ProfilerCategory.Network;
    public const string BandwidthBytesPerSecondName = "Bandwidth (bytes/s)";
    public static readonly ProfilerCounterValue<float> BandwidthBytesPerSecond =
        new ProfilerCounterValue<float>(NetworkCategory, BandwidthBytesPerSecondName, ProfilerMarkerDataUnit.Count
             );

    public const string PacketRateName = "Packet rate (Packets/s)";
    public static readonly ProfilerCounterValue<float> PacketRate =
        new ProfilerCounterValue<float>(NetworkCategory, PacketRateName, ProfilerMarkerDataUnit.Count
             );
}

#endif