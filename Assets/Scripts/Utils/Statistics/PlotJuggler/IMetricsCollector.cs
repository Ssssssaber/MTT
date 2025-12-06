using System.Collections.Generic;
using UnityEngine;

public interface IMetricsCollector
{
    public Dictionary<string, float> GetMetrics();
}
