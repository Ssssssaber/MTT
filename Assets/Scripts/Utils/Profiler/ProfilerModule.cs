#if UNITY_EDITOR

 using Unity.Profiling;
 using Unity.Profiling.Editor;

[System.Serializable]
[ProfilerModuleMetadata("Network")] 
public class NetworkProfilerModuele : ProfilerModule
{
    static readonly ProfilerCounterDescriptor[] k_Counters = new ProfilerCounterDescriptor[]
    {
        new ProfilerCounterDescriptor(GameStatistics.BandwidthBytesPerSecondName, GameStatistics.NetworkCategory),
        new ProfilerCounterDescriptor(GameStatistics.PacketRateName, GameStatistics.NetworkCategory),
    };

    // Ensure that both ProfilerCategory.Scripts and ProfilerCategory.Memory categories are enabled when our module is active.
    static readonly string[] k_AutoEnabledCategoryNames = new string[]
    {
        ProfilerCategory.Scripts.Name,
        ProfilerCategory.Memory.Name
    };


    // Pass the auto-enabled category names to the base constructor.
    public NetworkProfilerModuele() : base(k_Counters, autoEnabledCategoryNames: k_AutoEnabledCategoryNames) { }
}

#endif