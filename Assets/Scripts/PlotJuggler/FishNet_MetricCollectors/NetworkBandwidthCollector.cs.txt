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
        [SerializeField] private float _updateInterval = 0.1f;
        private ulong _dataIn = 0;
        private ulong _dataOut = 0;
        private NetworkTrafficStatistics _networkTrafficStatistics;
        private bool _initialized = false;
        private byte _secondsAveraged = 1;

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

            float inKbps = (_dataIn / 1024f);
            float outKbps = (_dataOut / 1024f);

            dataDict.Add("Bandwidth/Received (KB)", inKbps);
            dataDict.Add("Bandwidth/Sent (KB)", outKbps);

            // Reset counters
            _dataIn = 0;
            _dataOut = 0;

            return dataDict;
        }

        private void NetworkTrafficStatistics_OnNetworkTraffic(uint tick, BidirectionalNetworkTraffic serverTraffic, BidirectionalNetworkTraffic clientTraffic)
        {
            if (!_initialized)
                return;

            // Accumulate traffic data
            if (InstanceFinder.IsServerStarted)
            {
                _dataIn += serverTraffic.GetInboundTraffic();
                _dataOut += serverTraffic.GetOutboundTraffic();
            }

            if (InstanceFinder.IsClientStarted)
            {
                _dataIn += clientTraffic.GetInboundTraffic();
                _dataOut += clientTraffic.GetOutboundTraffic();
            }
        }
    }
}
