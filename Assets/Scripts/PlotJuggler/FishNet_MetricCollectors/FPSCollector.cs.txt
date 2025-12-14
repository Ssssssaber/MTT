using System.Collections.Generic;
using UnityEngine;

public class FPSCollector : MonoBehaviour, IMetricsCollector
{
    public Dictionary<string, float> GetMetrics()
    {
        return new Dictionary<string, float>() { { "FPS", 1 / Time.deltaTime } };
    }
}
