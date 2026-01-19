using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PlotJugglerUDPJSONSender : MonoBehaviour
{
    [Header("PlotJuggler Connection Settings")]
    public string plotJugglerIP = "127.0.0.1";
    public int plotJugglerPort = 9000;
    public float sendInterval = 0.5f;

    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;
    private bool isConnected = false;
    private List<IMetricsCollector> _metricsCollectors = new List<IMetricsCollector>();
    private Coroutine sendCoroutine;
    private Rect windowRect = new Rect(10, Screen.height - 200, 250, 190);
    private string clientID;

    private void Awake()
    {
        FindMetricsCollectors();
        GameManager.ArgumentsInitialized += InitializeConnection;
    }

	private void InitializeConnection()
    {
        var args = GameManager.Instance.GetCommandLineArguments(); 
        clientID = ConnectionInfo.GetConnectionInfo();
        if (args.isClient || args.isServer)
        {
            ConnectToPlotJuggler();
        }
        MirrorGameManager.RecordingStopped += OnRecordingStopped;
    }

	private void OnRecordingStopped()
    {
        SendStopRecordingMessage();
    }

    private void SendStopRecordingMessage()
    {
        var stopMessage = new JObject
        {
            ["stop_recording"] = true,
            ["client_id"] = clientID
        };
        SendJSONMessage(stopMessage.ToString(Formatting.None));
    }

    private void FindMetricsCollectors()
    {
        _metricsCollectors.Clear();
        var mcol = GetComponentsInChildren<IMetricsCollector>(true);
        Debug.Log($"Found {mcol.Count()} metrics collectors.");
        _metricsCollectors = mcol.ToList();
    }

    private void OnDestroy()
    {
        if (isConnected)
        {
            DisconnectFromPlotJuggler();
        }
        if (sendCoroutine != null)
        {
            StopCoroutine(sendCoroutine);
        }
    }

    private void ConnectToPlotJuggler()
    {
        if (string.IsNullOrEmpty(plotJugglerIP) || plotJugglerPort <= 0)
        {
            Debug.LogError("Invalid PlotJuggler IP or Port.");
            return;
        }

        try
        {
            udpClient = new UdpClient();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(plotJugglerIP), plotJugglerPort);
            isConnected = true;
            Debug.Log("Connected to PlotJuggler via UDP!");

            sendCoroutine = StartCoroutine(SendMetricsCoroutine());
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to connect to PlotJuggler: {e.Message}");
            isConnected = false;
        }
    }

    private void DisconnectFromPlotJuggler()
    {
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
        isConnected = false;
        Debug.Log("Disconnected from PlotJuggler.");
    }

    private System.Collections.IEnumerator SendMetricsCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(sendInterval);
        while (isConnected)
        {
            string jsonMessage = CreatePlotJugglerJSON();
            SendJSONMessage(jsonMessage);
            yield return wait;
        }
    }

    private void SendJSONMessage(string message)
    {
        if (!isConnected || udpClient == null)
            return;

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send data to PlotJuggler: {e.Message}");
            isConnected = false;
        }
    }

    private string CreatePlotJugglerJSON()
    {
        Dictionary<string, float> fullMetrics = new Dictionary<string, float>();

        foreach (IMetricsCollector collector in _metricsCollectors)
        {
            Dictionary<string, float> metrics = collector.GetMetrics();
            foreach (KeyValuePair<string, float> metric in metrics)
            {
                string flattenedKey = metric.Key.Replace(".", "_");
                if (!fullMetrics.ContainsKey(flattenedKey))
                {
                    fullMetrics.Add(flattenedKey, metric.Value);
                }
            }
        }

        var dataObject = new JObject
        {
            ["timestamp"] = Time.time,
            ["client_id"] = clientID,
            ["data"] = JObject.FromObject(fullMetrics.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
        };

        return dataObject.ToString(Formatting.None);
    }

    private void OnGUI()
    {
        windowRect = GUILayout.Window(0, windowRect, DrawConnectionWindow, "PlotJuggler Settings");
    }

    private void DrawConnectionWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.Label("Connection Settings");

        GUILayout.BeginHorizontal();
        GUILayout.Label("IP Address:", GUILayout.Width(80));
        plotJugglerIP = GUILayout.TextField(plotJugglerIP);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Port:", GUILayout.Width(80));
        string portInput = GUILayout.TextField(plotJugglerPort.ToString());
        if (int.TryParse(portInput, out int newPort))
        {
            plotJugglerPort = newPort;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Send Interval:", GUILayout.Width(80));
        string intervalInput = GUILayout.TextField(sendInterval.ToString(CultureInfo.InvariantCulture));
        if (float.TryParse(intervalInput, out float newInterval))
        {
            sendInterval = newInterval;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Client ID:", GUILayout.Width(80));
        clientID = GUILayout.TextField(clientID);
        GUILayout.EndHorizontal();

        if (isConnected)
        {
            if (GUILayout.Button("Connected to PlotJuggler"))
            {
                DisconnectFromPlotJuggler();
            }
        }
        else
        {
            if (GUILayout.Button("Connect to PlotJuggler"))
            {
                ConnectToPlotJuggler();
            }
        }

        GUILayout.EndVertical();

        // Allow dragging the window
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}
