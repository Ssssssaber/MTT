using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

namespace DI
{
    public class SimulationManager : MonoBehaviour
    {
        Dictionary<ulong, UpdateableBehaviour> _updatables = new Dictionary<ulong, UpdateableBehaviour>();

        public void Register(UpdateableBehaviour updatable)
        {
            if (_updatables.ContainsKey(updatable.GetUID()))
            {
                MainLogger.instance.Error("Updatable already registered: " + updatable);
                return;
            }

            _updatables.Add(updatable.GetUID(), updatable);
        }
        
        public void Unregister(UpdateableBehaviour updatable)
        {
            _updatables.Remove(updatable.GetUID());
        }

        private void FixedUpdate()
        {
            UpdateSimulation(Time.fixedDeltaTime);
        }

        private void UpdateSimulation(float deltaTime)
        {
            foreach (var updatable in _updatables.Values)
            {
                updatable.SimulationUpdate(deltaTime);
            }
        }
    }
}
