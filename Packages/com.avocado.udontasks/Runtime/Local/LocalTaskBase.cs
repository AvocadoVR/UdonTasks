
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine;
using System;


namespace UdonTasks.Runtime.Local
{
    public abstract class LocalTaskBase : UdonSharpBehaviour
    {
        [HideInInspector] public int currentTask = -1;
        [SerializeField] public int taskAmount = 0;

        private bool blockContinue;

        public bool isTaskRunning { get; private set; }

        public virtual void StartTask()
        {
            isTaskRunning = true;
            Task();
        }

        public virtual void StopTask()
        {
            isTaskRunning = false;
            currentTask = -1;
        }

        public virtual void Task() { }


        public virtual void Continue()
        {
            if (!isTaskRunning || currentTask == taskAmount || blockContinue) return;

            currentTask++;
            Task();

        }

        public virtual void WaitForSeconds(int seconds)
        {
            if (!isTaskRunning || currentTask == taskAmount) return;
            blockContinue = true;

            SendCustomEventDelayedSeconds(nameof(WaitForSecondsCallback), seconds);
        }

        public virtual void WaitForSecondsCallback()
        {
            if (!isTaskRunning || currentTask == taskAmount) return;

            blockContinue = false;

            currentTask++;
            Task();
        }
    }
}