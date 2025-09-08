using TMPro;
using System.Collections.Concurrent;
using Serilog;
using Serilog.Core;
using System;
using Serilog.Events;
using Serilog.Formatting;  // Add this for ITextFormatter
using Serilog.Formatting.Display;  // Add this for MessageTemplateTextFormatter
using System.IO;  // For StringWriter

public class UILogSinkBackend : ILogEventSink
{
    private readonly IFormatProvider _formatProvider;
    private readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
    private TMP_InputField _logField;
    private readonly ITextFormatter _formatter;  // Formatter for the output template

    public UILogSinkBackend(IFormatProvider formatProvider, TMP_InputField logField, string outputTemplate)
    {
        _logField = logField;
        _formatProvider = formatProvider;  // Default if null
        _formatter = new MessageTemplateTextFormatter(outputTemplate, _formatProvider);
    }
    
    public void Emit(LogEvent logEvent)
    {
        // Format the log event using the template
        using (var writer = new StringWriter())
        {
            _formatter.Format(logEvent, writer);
            string formattedMessage = writer.ToString().TrimEnd();  // Trim trailing newline if needed
            _logQueue.Enqueue(formattedMessage);
        }
    }

    public void Update()
    {
        while (_logQueue.TryDequeue(out var log))
        {
            _logField.text += log + "\n";  // Add newline for separation
            _logField.caretPosition = _logField.text.Length;
        }
    }
}
