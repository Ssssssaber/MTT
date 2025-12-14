using Fusion;
using Fusion.Statistics;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBandwidthCollector : MonoBehaviour, IMetricsCollector
{
    private float dataIn = -1;
    private float dataOut = -1;
    private float lastDataIn = -1;
    private float lastDataOut = -1;
    private FusionStatisticsManager _stats;
    [SerializeField] NetworkRunner _runner;

    private void Start()
    {
        _runner.TryGetFusionStatistics(out _stats);
    }

    void OnEnable()
    {
        // If we've been inactive, clear counter
        dataIn = -1;
        dataOut = -1;
        lastDataIn = -1;
        lastDataOut = -1;
    }

    public Dictionary<string, float> GetMetrics()
    {
        if (_runner.TryGetFusionStatistics(out var statisticsManager))
        {
            dataIn = statisticsManager.CompleteSnapshot.InBandwidth;
            dataOut = statisticsManager.CompleteSnapshot.OutBandwidth;
            if (dataIn > 0 || dataOut > 0)
            {
                lastDataIn = dataIn;
                lastDataOut = dataOut;
            }
        }
        else
        {
            Debug.LogError("Bandwidth stat man null!");
        }

        var dataDict = new Dictionary<string, float>() {
            { "Bandwidth/Received (KB)", lastDataIn / 1024.0f },
            { "Bandwidth/Sent (KB)",     lastDataOut / 1024.0f }
        };

        return dataDict;
    }
}
