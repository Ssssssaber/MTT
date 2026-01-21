using FishNet;
using System.Collections.Generic;
using UnityEngine;

public class RTTCollector : MonoBehaviour, IMetricsCollector 
{
    public Dictionary<string, float> GetMetrics()
    {
        return new Dictionary<string, float>() {
            { "RTT (ms)", (float)InstanceFinder.TimeManager.RoundTripTime }
        };
    }
}
