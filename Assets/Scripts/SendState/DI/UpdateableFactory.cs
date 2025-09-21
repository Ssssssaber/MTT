using UnityEngine;
using VContainer.Unity;

namespace SendState.DI
{
    public class UpdateableFactory
    {
        private readonly LifetimeScope _lifetime;
        public UpdateableFactory(LifetimeScope lifetime)
        {
            _lifetime = lifetime;
        }

        public T Create<T>(GameObject prefab, Transform parent, ulong uid) where T : UpdateableBehaviour
        {
            GameObject obj = null;

            if (parent == null)
                obj = Object.Instantiate(prefab);
            else
                obj = Object.Instantiate(prefab, parent);

            _lifetime.Container.InjectGameObject(obj);
            var behaviour = obj.GetComponent<T>();
            behaviour.Register(uid);
            return behaviour;
        }

        public void RegisterAlreadyCreated(UpdateableBehaviour behaviour, ulong uid)
        {
            _lifetime.Container.InjectGameObject(behaviour.gameObject);
            behaviour.Register(uid);
        }
    }
}
