using UnityEngine;

namespace SendState.DI
{
    public class UpdateableFactory
    {
        private readonly LifetimeScope _lifetime;
        public UpdateableFactory(LifetimeScope lifetime)
        {
            _lifetime = lifetime;
        }

        public T Create<T>(GameObject prefab, Transform parent, ulong uid) where T : UpdatableBehaviour
        {
            GameObject obj = null;
            if (parent == null)
                var obj = UnityEngine.Object.Instantiate(prefab);
            else
                var obj = UnityEngine.Object.Instantiate(prefab, parent);

            _lifetime.Container.InjectGameObject(obj);
            var behaviour = obj.GetComponent<T>();
            behaviour.SetUID(uid);
            return behaviour;
        }
    }
}
