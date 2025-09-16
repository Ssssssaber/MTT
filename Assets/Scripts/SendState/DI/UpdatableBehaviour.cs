using VContainer;
using VContainer.Unity;

namespace SendState.DI
{
    using SendState.MirrorNetwork;
    using UnityEngine;

    public class UpdateableBehaviour : MonoBehaviour, IUpdatable
    {
        private SimulationManager _manager;
        protected ulong UID = 0;

        public virtual ObjectRepresentation GetRepresentation()
        {
            return ObjectRepresentation.None;
        }

        public ulong GetUID()
        {
            return UID;
        }

        [Inject]
        public void Construct(SimulationManager manager, ulong uid = 0)
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
