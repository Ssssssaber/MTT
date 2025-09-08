using UnityEngine;
using Serilog;
using TMPro;
using System;

public class MainLogger : MonoBehaviour
{
    public static MainLogger instance { get; private set; }
    [SerializeField] TMP_InputField _logSink;
    private UILogSinkBackend _logSinkBackend;
    private Serilog.Core.Logger _logger;
    [SerializeField] private string _outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
    public void Info(string message)
    {
        _logger.Information(message);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        var datetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");


        _logSinkBackend = new UILogSinkBackend(null, _logSink, _outputTemplate);
        _logger = new LoggerConfiguration()
            .WriteTo.File($"UserLogs/{datetime}.txt", outputTemplate: _outputTemplate)
            .WriteTo.Sink(_logSinkBackend)
            .CreateLogger();
        
        Debug.Log("Logger Initialized: " + $"UserLogs/{datetime}.txt");
    }

    void Update()
    {
        _logSinkBackend.Update();
    }
}
