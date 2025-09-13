using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class FrameStatisticsCounter : MonoBehaviour
{
    [SerializeField] private const float _perTime = 1.0f;
    private float _passedTime = 0.0f;
    private ulong _fpsFrameCount = 0;
    private FrameStatistics _stats = new FrameStatistics();

    public UnityEvent<FrameStatistics> StatisticsUpdated = new UnityEvent<FrameStatistics>();

    public FrameStatistics GetStatistics() { return _stats; }

    public void UpdateStats(float deltaTime)
    {
        _stats.frameCount++;
        UpdateFPS(deltaTime);
    }

    // Update is called once per frame
    private void UpdateFPS(float deltaTime)
    {
        _fpsFrameCount++;
        _passedTime += deltaTime;

        if (_passedTime < _perTime) return;

        _stats.fps = _fpsFrameCount / _passedTime;
        StatisticsUpdated.Invoke(_stats);

        _fpsFrameCount = 0;
        _passedTime = 0;
    }
}
