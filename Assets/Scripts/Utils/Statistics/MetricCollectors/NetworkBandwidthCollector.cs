using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Editing;
using FishNet.Managing;
using FishNet.Managing.Statistic;
using UnityEngine;

namespace FishNet.Component.Utility
{
    public class NetworkBandwidthCollector : MonoBehaviour, IMetricsCollector
    {
        private ulong _dataIn = 0;
        private ulong _dataOut = 0;
        private NetworkTrafficStatistics _networkTrafficStatistics;
        private bool _initialized = false;
        private byte _secondsAveraged = 1;
        private float _lastUpdateTime = 0f;
        private float _updateInterval = 1f;

        private void Start()
        {
            if (!InstanceFinder.NetworkManager.StatisticsManager.TryGetNetworkTrafficStatistics(out _networkTrafficStatistics))
                return;

            if (!_networkTrafficStatistics.UpdateClient && !_networkTrafficStatistics.UpdateServer)
            {
                Debug.LogWarning($"StatisticsManager.NetworkTraffic is not updating for client nor server. " +
                    "To see results ensure your NetworkManager has a StatisticsManager component added " +
                    "with the NetworkTraffic values configured.");
                return;
            }

            SetSecondsAveraged(_secondsAveraged);
            _networkTrafficStatistics.OnNetworkTraffic += NetworkTrafficStatistics_OnNetworkTraffic;
            _initialized = true;
            _lastUpdateTime = Time.time;
        }

        void OnEnable()
        {
            _dataIn = 0;
            _dataOut = 0;
        }

        void OnDestroy()
        {
            if (_networkTrafficStatistics != null)
                _networkTrafficStatistics.OnNetworkTraffic -= NetworkTrafficStatistics_OnNetworkTraffic;
            _initialized = false;
        }

        /// <summary>
        /// Sets a new number of seconds to average from.
        /// </summary>
        public void SetSecondsAveraged(byte seconds)
        {
            _secondsAveraged = seconds;
        }

        public Dictionary<string, float> GetMetrics()
        {
            var dataDict = new Dictionary<string, float>();

            // Calculate KB/s
            float timeSinceLastUpdate = Time.time - _lastUpdateTime;

            if (timeSinceLastUpdate >= _updateInterval)
            {
                float inKbps = (_dataIn / 1024f) / timeSinceLastUpdate;
                float outKbps = (_dataOut / 1024f) / timeSinceLastUpdate;

                dataDict.Add("Bandwidth/Received (KB/s)", inKbps);
                dataDict.Add("Bandwidth/Sent (KB/s)", outKbps);

                // Reset counters
                _dataIn = 0;
                _dataOut = 0;
                _lastUpdateTime = Time.time;
            }
            else
            {
                // Return zeros if not enough time has passed
                dataDict.Add("Bandwidth/Received (KB/s)", 0f);
                dataDict.Add("Bandwidth/Sent (KB/s)", 0f);
            }

            return dataDict;
        }

        private void NetworkTrafficStatistics_OnNetworkTraffic(uint tick, BidirectionalNetworkTraffic serverTraffic, BidirectionalNetworkTraffic clientTraffic)
        {
            if (!_initialized)
                return;

            // Accumulate traffic data
            if (InstanceFinder.IsServer)
            {
                _dataIn += serverTraffic.GetInboundTraffic();
                _dataOut += serverTraffic.GetOutboundTraffic();
            }

            if (InstanceFinder.IsClient)
            {
                _dataIn += clientTraffic.GetInboundTraffic();
                _dataOut += clientTraffic.GetOutboundTraffic();
            }
        }
    }
}
