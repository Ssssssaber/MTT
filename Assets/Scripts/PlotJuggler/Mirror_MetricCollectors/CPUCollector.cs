using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CPUCollector : MonoBehaviour, IMetricsCollector
{
    private Process currentProcess;
    private TimeSpan previousProcessTime;
    private DateTime previousTime;
    private float cpuUsage;

    void Start()
    {
        currentProcess = Process.GetCurrentProcess();

        // Take an initial sample
        previousProcessTime = currentProcess.TotalProcessorTime;
        previousTime = DateTime.UtcNow;

        // Update every 1 second
        InvokeRepeating("UpdateCPUUsage", 1f, 1f);
    }

    void UpdateCPUUsage()
    {
        var currentProcessTime = currentProcess.TotalProcessorTime;
        var currentTime = DateTime.UtcNow;

        // calculate the difference in processor time and wall clock time
        var cpuUsedMs = (currentProcessTime - previousProcessTime).TotalMilliseconds;
        var totalMsPassed = (currentTime - previousTime).TotalMilliseconds;

        // calculate the CPU usage percentage
        // divide by Environment.ProcessorCount to get the usage percentage across all cores
        cpuUsage = (float)Math.Round(cpuUsedMs / (Environment.ProcessorCount * totalMsPassed) * 100, 2);

        // update previous values for the next calculation
        previousProcessTime = currentProcessTime;
        previousTime = currentTime;
    }

    public Dictionary<string, float> GetMetrics()
    {
        return new Dictionary<string, float>() { { "CPU Usage (%)", cpuUsage } };
    }
}
