using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLogs : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField _logField;

    private void Awake()
    {
        MainLogger.RegisterListener(AppendLog);
    }

    private void AppendLog(string log)
    {
        _logField.text += log + "\n";
        _logField.caretPosition = _logField.text.Length;
    }
}
