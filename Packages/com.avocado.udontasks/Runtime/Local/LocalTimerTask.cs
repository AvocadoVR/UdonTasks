using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.Udon;

public abstract class LocalTimerTask : UdonSharpBehaviour
{
    public int minutes { get; private set; }
    public int seconds { get; private set; }

    public bool isTimerRunning { get; private set; }

    public virtual void StartTimer()
    {
        isTimerRunning = true;

        SendCustomEventDelayedSeconds(nameof(Timer), 1);
    }

    public virtual void Timer()
    {
        if (!isTimerRunning)
        {
            return;
        }

        if (seconds > 0)
        {
            seconds--;
            OnTimerTick();
            SendCustomEventDelayedSeconds(nameof(Timer), 1);
        }
        else if (seconds == 0 && minutes > 0)
        {
            minutes--;
            seconds = 59;
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
        isTimerRunning = false;
        minutes = 0;
        seconds = 0;
        OnTimerStop();
    }


    public void SetTimer(int _min, int _sec)
    {
        minutes = _min;
        seconds = _sec;
    }
}
