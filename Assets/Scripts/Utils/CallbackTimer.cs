using System;

public delegate void OnTimerUpdate();

public class CallbackTimer
{
    private float _timePassed = 0.0f;
    private float _updateTime = 1.0f;
    private OnTimerUpdate _updateCallback;

    public CallbackTimer(OnTimerUpdate updateCallback, float updateTime = 1.0f)
    {
        _updateTime = updateTime;
        _updateCallback = updateCallback;
    }

    public ref OnTimerUpdate GetUpdateCallback()
    {
        return ref _updateCallback;
    }

    public void Update(float deltaTime)
    {
        _timePassed += deltaTime;
        if (_timePassed < _updateTime) return;

        _updateCallback();
        _timePassed = 0.0f;
    }
}