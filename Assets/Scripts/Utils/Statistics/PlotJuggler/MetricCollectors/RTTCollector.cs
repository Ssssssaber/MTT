using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class RTTCollector : MonoBehaviour, IMetricsCollector 
{
    public Dictionary<string, float> GetMetrics()
    {
        return new Dictionary<string, float>() {
            { "RTT (ms)", (float)NetworkTime.rtt * 1000f }
        };
    }
}
