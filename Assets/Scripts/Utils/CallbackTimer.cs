using System;

public delegate void OnTimerUpdate();

public class CallbackTimer
{
    private float _timePassed = 0.0f;
    private float _updateTime = 1.0f;
    private bool _enabled = false;
    private OnTimerUpdate _updateCallback;

    public void SetEnabled(bool enabled, bool resetTimer = false)
    {
        if (!enabled && resetTimer)
        {
            _timePassed = 0.0f;
        }

        _enabled = enabled;
    }

    public CallbackTimer(OnTimerUpdate updateCallback, float updateTime = 1.0f, bool enabled = true)
    {
        _updateTime = updateTime;
        _updateCallback = updateCallback;
        _enabled = enabled;
    }

    public ref OnTimerUpdate GetUpdateCallback()
    {
        return ref _updateCallback;
    }

    public void Update(float deltaTime)
    {
        if (!_enabled) return;

        _timePassed += deltaTime;
        if (_timePassed < _updateTime) return;

        _updateCallback();
        _timePassed = 0.0f;
    }
}