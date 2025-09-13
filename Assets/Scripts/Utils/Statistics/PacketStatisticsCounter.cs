using UnityEngine;
using UnityEngine.Events;

public class PacketStatisticsCounter : MonoBehaviour
{
    [SerializeField] private const float _perTime = 1.0f;
    private float _passedTime = 0.0f;
    private uint _packetCount = 0;

    private float _fps = 0.0f;
    [SerializeField] private UnityEvent<float> fpsReady = new UnityEvent<float>();

    public float GetFPS() { return _fps; }

    // Update is called once per frame
    private void UpdateFPS(float deltaTime)
    {
        _packetCount++;
        _passedTime += deltaTime;

        if (_passedTime < _perTime) return;

        _fps = _packetCount / _passedTime;
        fpsReady.Invoke(_fps);

        _packetCount = 0;
        _passedTime = 0;
    }
}

