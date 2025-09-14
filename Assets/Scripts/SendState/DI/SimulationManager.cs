using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;
using Mirror;

using SendState.MirrorNetwork;

namespace SendState.DI
{
    public class SimulationManager : NetworkBehaviour
    {
        Dictionary<ulong, UpdateableBehaviour> _updatables = new Dictionary<ulong, UpdateableBehaviour>();
        GameSnapshot _snapshot;

        private void Awake()
        {
            _snapshot = new GameSnapshot(0);
        }

        public override void OnStartClient()
        {
            if (isServer) return;

            base.OnStartClient();
            NetworkClient.RegisterHandler<GameSnapshotMessage>(OnGameSnapshotReceived);
        }

        private void OnGameSnapshotReceived(GameSnapshotMessage msg)
        {
            // Apply snapshot to your scene
            for (int i = 0; i < msg.IDs.Count; i++)
            {
                ulong id = msg.IDs[i];
                Vector3 pos = msg.Positions[i];
                Quaternion rot = msg.Rotations[i];

                var obj = _updatables.ContainsKey(id) ? _updatables[id].gameObject : null;
                if (obj != null)
                {
                    obj.transform.position = pos;
                    obj.transform.rotation = rot;
                    return;
                }

                GameManager.instance.SpawnMiniCube(pos, rot);
            }
        }

        public void Register(UpdateableBehaviour updatable)
        {
            if (!isServer) return;

            if (_updatables.ContainsKey(updatable.GetUID()))
            {
                MainLogger.instance.Error("Updatable already registered: " + updatable);
                return;
            }

            _updatables.Add(updatable.GetUID(), updatable);
        }

        public void Unregister(UpdateableBehaviour updatable)
        {
            if (!isServer) return;

            _updatables.Remove(updatable.GetUID());
        }

        private void FixedUpdate()
        {
            if (!isServer) return;

            UpdateSimulation(Time.fixedDeltaTime);
            UpdateSnapshot();
        }

        private void UpdateSimulation(float deltaTime)
        {
            foreach (var updatable in _updatables.Values)
            {
                updatable.SimulationUpdate(deltaTime);
            }
        }

        private void UpdateSnapshot()
        {

            _snapshot.Tick++;

            _snapshot.IDs.Clear();
            _snapshot.Positions.Clear();
            _snapshot.Rotations.Clear();
            _snapshot.Colors.Clear();

            foreach (var updatable in _updatables.Values)
            {
                _snapshot.IDs.Add(updatable.GetUID());
                _snapshot.Positions.Add(updatable.transform.position);
                _snapshot.Rotations.Add(updatable.transform.rotation);
                var renderer = updatable.GetComponent<Renderer>();
                if (renderer != null)
                {
                    _snapshot.Colors.Add(renderer.material.color);
                }
                else
                {
                    _snapshot.Colors.Add(Color.white);
                }
            }

            var msg = new GameSnapshotMessage
            {
                Tick = _snapshot.Tick,
                IDs = new List<ulong>(_snapshot.IDs),
                Positions = new List<Vector3>(_snapshot.Positions),
                Rotations = new List<Quaternion>(_snapshot.Rotations),
                Colors = new List<Color>(_snapshot.Colors)
            };

            if (NetworkServer.connections.Count == 0) return;
            NetworkServer.SendToAll(msg);
        }
    }
}
