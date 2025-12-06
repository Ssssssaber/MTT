using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBandwidthCollector : MonoBehaviour, IMetricsCollector
{
    private int dataIn = 0;
    private int dataOut = 0;

    void OnSend(NetworkDiagnostics.MessageInfo obj) => dataOut += obj.bytes;
    void OnReceive(NetworkDiagnostics.MessageInfo obj) => dataIn += obj.bytes;

    private void Start()
    {
        NetworkDiagnostics.InMessageEvent += OnReceive;
        NetworkDiagnostics.OutMessageEvent += OnSend;
    }

    void OnEnable()
    {
        // If we've been inactive, clear counter
        dataIn = 0;
        dataOut = 0;
    }

    void OnDestroy()
    {
        NetworkDiagnostics.InMessageEvent -= OnReceive;
        NetworkDiagnostics.OutMessageEvent -= OnSend;
    }

    public Dictionary<string, float> GetMetrics()
    {
        var dataDict = new Dictionary<string, float>() {
            { "Bandwidth/Received (KB)", dataIn / 1024.0f },
            { "Bandwidth/Sent (KB)",     dataOut / 1024.0f }
        };
        dataIn = 0;
        dataOut = 0;
        
        return dataDict;
    }
}
