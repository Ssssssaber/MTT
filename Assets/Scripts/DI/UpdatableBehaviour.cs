using VContainer;
using VContainer.Unity;

namespace DI
{
    using UnityEngine;

    public class UpdateableBehaviour : MonoBehaviour, IUpdatable
    {
        private SimulationManager _manager;
        protected ulong UID = UIDGenerator.GetID();
        public ulong GetUID()
        {
            return UID;
        }

        [Inject]
        public void Construct(SimulationManager manager)
        {
            _manager = manager;
            _manager.Register(this);  // Register on injection
        }

        public virtual void SimulationUpdate(float deltaTime)
        {

        }

        private void OnDestroy()
        {
            _manager.Unregister(this);
        }
    }   

}
