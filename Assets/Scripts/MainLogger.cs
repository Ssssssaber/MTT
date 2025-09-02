using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainLogger : MonoBehaviour
{
    private static UnityEvent<string> _logEvent = new UnityEvent<string>();

    public static void RegisterListener(UnityAction<string> listener)
    {
        _logEvent.AddListener(listener);
    }

    public static void Info(string message)
    {
        _logEvent.Invoke(String.Format("[{0}] I {1}", DateTime.Now.ToString("HH:mm:ss"), message));
    }
}
