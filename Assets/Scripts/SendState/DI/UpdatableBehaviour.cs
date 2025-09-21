using VContainer;
using SendState.MiniCubes;
using SendState.PlayerCube;

namespace SendState.DI
{
    using System;
    using SendState.MirrorNetwork;
    using UnityEngine;

    [Serializable]
    public class UpdateableBehaviour : MonoBehaviour, IUpdatable
    {
        protected SimulationManager _manager;
        protected ulong UID = 0;

        public virtual ObjectRepresentation GetRepresentation()
        {
            return ObjectRepresentation.None;
        }

        public ulong GetUID()
        {
            return UID;
        }

        public void SetUID(ulong uid)
        {
            UID = uid;
        }

        [Inject]
        public void InjectManager(SimulationManager manager)
        {
            _manager = manager;
        }

        public virtual void Register(ulong uid = 0)
        {
            if (uid == 0)
                UID = UIDGenerator.GetID();
            else
                UID = uid;

            // Register additional updatables
            var movement = GetComponent<MovementHandler>();
            if (movement != null)
            {
                movement.SetUID(UIDGenerator.GetID());
                _manager.Register(movement);
            }

            var colorState = GetComponent<ColorState>();
            if (colorState != null)
            {
                colorState.SetUID(UIDGenerator.GetID());
                _manager.Register(colorState);
            }

            var camera = GetComponent<CameraFollow>();
            if (colorState != null)
            {
                camera.SetUID(UIDGenerator.GetID());
                _manager.Register(camera);
            }
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
