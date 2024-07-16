using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.Udon;

public abstract class SyncedTimerTask : UdonSharpBehaviour
{
    [UdonSynced] private int Minutes;
    [UdonSynced] private int Seconds;

    [UdonSynced] private bool IsTimerRunning;

    public int minutes => Minutes;
    public int seconds => Seconds;
    public bool isTimerRunning => IsTimerRunning;

    public virtual void StartTimer()
    {
        IsTimerRunning = true;
        RequestSerialization();

        SendCustomEventDelayedSeconds(nameof(Timer), 1);
    }

    public virtual void Timer()
    {
        if (!IsTimerRunning)
        {
            return;
        }

        if (seconds > 0)
        {
            Seconds--;
            RequestSerialization();
            OnTimerTick();
            SendCustomEventDelayedSeconds(nameof(Timer), 1);
        }
        else if (seconds == 0 && minutes > 0)
        {
            Minutes--;
            Seconds = 59;
            RequestSerialization();
            OnTimerTick();
            SendCustomEventDelayedSeconds(nameof(Timer), 1);
        }
        else
        {
            StopTimer();
            OnTimerCompletion();
        }
    }

    public virtual void OnTimerTick() { }

    public virtual void OnTimerCompletion() { }

    public virtual void OnTimerStop() { }

    public virtual void StopTimer()
    {
        IsTimerRunning = false;
        Minutes = 0;
        Seconds = 0;
        RequestSerialization();
        OnTimerStop();
    }

    public void SetTimer(int _min, int _sec)
    {
        Minutes = _min;
        Seconds = _sec;
        RequestSerialization();
    }
}
