
namespace DI
{
    public interface IUpdatable
    {
        ulong GetUID();
        void SimulationUpdate(float deltaTime);
    }
}