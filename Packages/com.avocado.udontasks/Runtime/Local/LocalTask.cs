
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine;
using System;

namespace UdonTasks.Runtime.Local
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class LocalTask : UdonSharpBehaviour
    {
        private bool isConditionMet = false;
        public bool isTaskRunning { get; private set; } = false;

        #region Set Code
        public void SetCondition(bool Condition)
        {
            if (!repeatUntilMet[currentTask])
            {
                isConditionMet = Condition;
                ConditonalBreak();
            }
            else
            {
                isConditionMet = Condition;
                ConditonalRepeat();
            }
        }
        #endregion

        private int currentTask = -1;
        private int taskCount;

        public TypeOfFunction[] functions;

        public string[] methodNames;
        public UdonBehaviour[] callbacks;

        public bool[] manualConfirmation;
        public bool[] repeatUntilMet;

        public int[] waitForSeconds;

        void Start()
        {
            taskCount = functions.Length - 1;
        }

        public void StartTask()
        {
            isTaskRunning = true;
            TaskFunc();
        }

        public void StopTask()
        {
            isTaskRunning = false;
            currentTask = -1;
            isConditionMet = false;
        }

        private void TaskFunc()
        {
            if (!isTaskRunning) return;

            currentTask++;

            var methodName = methodNames[currentTask];
            var callback = callbacks[currentTask];
            var taskFunction = functions[currentTask];

            if (callback == null || string.IsNullOrEmpty(methodName) || string.IsNullOrWhiteSpace(methodName))
            {
                StopTask();
                return;
            }
            else if (taskFunction == TypeOfFunction.WaitForSeconds && currentTask == taskCount)
            {
                StopTask();
                return;
            }

            switch (taskFunction)
            {
                case TypeOfFunction.Task:
                    callback.SendCustomEvent(methodNames[currentTask]);

                    if (manualConfirmation[currentTask]) return;
                    break;
                case TypeOfFunction.WaitForSeconds:

                    SendCustomEventDelayedSeconds(nameof(WaitForSecondsCallback), waitForSeconds[currentTask]);
                    return;
                case TypeOfFunction.Condition:

                    callback.SendCustomEvent(methodName);
                    return;
            }


            if (currentTask < taskCount)
            {
                TaskFunc();
            }
            else if (currentTask == taskCount)
            {
                StopTask();
            }
        }

        public void Continue()
        {
            if (!manualConfirmation[currentTask] || currentTask == taskCount || !isTaskRunning)
            {
                StopTask();
                return;
            }

            TaskFunc();
        }


        private void ConditonalBreak()
        {
            if (isConditionMet || currentTask == taskCount || functions[currentTask] != TypeOfFunction.Condition || !isTaskRunning) // Prevents people from causing errors.
            {
                StopTask();
                return;
            }

            TaskFunc();
        }

        private void ConditonalRepeat()
        {
            if (currentTask == taskCount || functions[currentTask] != TypeOfFunction.Condition || !isTaskRunning)
            {
                StopTask();
                return;
            }

            if (!isConditionMet)
            {
                callbacks[currentTask].SendCustomEvent(methodNames[currentTask]);
            }
            else if (isConditionMet)
            {
                TaskFunc();
            }
        }



        public void WaitForSecondsCallback()
        {
            if (!isTaskRunning || functions[currentTask] != TypeOfFunction.WaitForSeconds) return;

            TaskFunc();
        }
    }
}