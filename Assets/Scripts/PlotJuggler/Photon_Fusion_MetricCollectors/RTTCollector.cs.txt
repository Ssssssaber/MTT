using Fusion.Statistics;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class RTTCollector : MonoBehaviour, IMetricsCollector 
{
    private float lastRTT = 0;
    private FusionStatisticsManager _stats;
    [SerializeField] NetworkRunner _runner;

    private void Start()
    {
        _runner.TryGetFusionStatistics(out _stats);
    }

    void OnEnable()
    {
        // If we've been inactive, clear counter
        lastRTT = 0;
    }
    
    public Dictionary<string, float> GetMetrics()
    {
        float rtt = -1.0f;
        if (_runner.TryGetFusionStatistics(out var statisticsManager))
        {
            rtt = (float)statisticsManager.CompleteSnapshot.RoundTripTime * 1000f;
            if (rtt > 0.0f)
            {
                lastRTT = rtt;
            }
        }
        else
        {
            Debug.LogError("RTT stat man null!");
        }

        return new Dictionary<string, float>() {
            { "RTT (ms)",  lastRTT}
        };
    }
}
