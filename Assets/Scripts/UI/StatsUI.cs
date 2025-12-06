using FishNet;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _field;
    private CallbackTimer _timer;

    private void Start()
    {
        _timer = new CallbackTimer(this.UpdateText, 1.0f);
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private FrameStatistics _frame;
    void UpdateText()
    {
        _frame = StatisticsManager.instance.FrameCounter.GetStatistics();
        
        if (InstanceFinder.IsClientOnlyStarted)
        _field.text = $"Frame: {_frame.frameCount}\nFPS: {_frame.fps}"; 
    }
}
