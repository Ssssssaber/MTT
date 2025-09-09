#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilerUpdate : MonoBehaviour
{
    private float packets = 0;
    private const float packetSize = 8;

    private float passedTime = 0.0f;
    private float updateTime = 1.0f;
    private void Update()
    {
        passedTime += Time.deltaTime;

        if (passedTime < updateTime) return;

        packets = Random.Range(3.0f, 10.0f);
        GameStatistics.PacketRate.Value = packets;
        GameStatistics.BandwidthBytesPerSecond.Value = packetSize * packets;

        passedTime = 0.0f;
    }
}

#endif 