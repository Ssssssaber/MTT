using System.Security.Cryptography;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager instance { get; private set; }
    public FrameStatisticsCounter FrameCounter { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        FrameCounter = GetComponent<FrameStatisticsCounter>();
    }

    private void Update()
    {
        FrameCounter.UpdateStats(Time.deltaTime);
    } 
}