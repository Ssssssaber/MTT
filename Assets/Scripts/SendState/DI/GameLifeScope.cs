using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace SendState.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register the manager
            builder.RegisterComponentInHierarchy<SimulationManager>();
            builder.Register<UpdateableBehaviour>(Lifetime.Singleton);
        }
    }
}
