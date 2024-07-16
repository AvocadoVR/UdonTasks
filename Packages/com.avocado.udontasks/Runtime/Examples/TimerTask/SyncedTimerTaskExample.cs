
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedTimerTaskExample : SyncedTimerTask
{
    [SerializeField] private TextMeshProUGUI time;

    public void StartIt()
    {
        SetTimer(1, 10); // 1 Min 20 Seconds
        StartTimer();
    }


    public override void OnTimerTick() // Triggers every timer decreases.
    {
        UpdateTime();
    }

    public override void OnTimerCompletion() // Triggers when the timer is done.
    {
        Debug.Log("Timer Completed");
    }

    public override void OnTimerStop() // Triggers when the timer is stopped
    {
        UpdateTime();
    }

    public void UpdateTime()
    {
        time.text = $"{minutes} : {seconds}".ToString();
    }

    public override void OnDeserialization()
    {
        UpdateTime();
    }
}
