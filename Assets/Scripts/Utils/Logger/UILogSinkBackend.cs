using TMPro;
using System.Collections.Concurrent;
using Serilog;
using Serilog.Core;
using System;
using Serilog.Events;

public class UILogSinkBackend : ILogEventSink
{
    private readonly IFormatProvider _formatProvider;
    private readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
    private TMP_InputField _logField;

    public UILogSinkBackend(IFormatProvider formatProvider, TMP_InputField logField)
    {
        _logField = logField;
        _formatProvider = formatProvider;
    }
    
    public void Emit(LogEvent logEvent)
    {
         var message = logEvent.RenderMessage(_formatProvider);
        _logQueue.Enqueue(message);
    }

    public void Update()
    {
        while (_logQueue.TryDequeue(out var log))
        {
            _logField.text += log + "\n";
            _logField.caretPosition = _logField.text.Length;
        }
    }
}