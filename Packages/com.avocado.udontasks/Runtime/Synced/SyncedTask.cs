
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine;

namespace UdonTasks.Runtime.Synced
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncedTask : UdonSharpBehaviour
    {
        private bool isConditionMet = false;

        [UdonSynced] private bool IsTaskRunning = false;

        #region Set Code
        public bool isTaskRunning => IsTaskRunning;

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

        [UdonSynced] private int currentTask = -1;
        private int taskCount;

        public TypeOfFunction[] functions;

        public string[] methodNames;
        public UdonBehaviour[] callbacks;

        public bool[] manualConfirmation;
        public bool[] repeatUntilMet;

        public int[] waitForSeconds;

        private VRCPlayerApi player;

        void Start()
        {
            player = Networking.LocalPlayer;
            taskCount = functions.Length - 1;
        }

        public void StartTask()
        {
            IsTaskRunning = true;
            RequestSerialization();
            TaskFunc();
        }

        public void StopTask()
        {
            IsTaskRunning = false;
            currentTask = -1;
            RequestSerialization();

            isConditionMet = false;
        }

        private void TaskFunc()
        {
            if (!isTaskRunning || !Utilities.IsValid(player))
            {
                StopTask();
                return;
            }

            currentTask++;
            RequestSerialization();

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

                    if (!Networking.IsOwner(callback.gameObject)) Networking.SetOwner(player, callback.gameObject);

                    callback.SendCustomEvent(methodName);

                    if (manualConfirmation[currentTask]) return;
                    break;
                case TypeOfFunction.WaitForSeconds:

                    SendCustomEventDelayedSeconds(nameof(WaitForSecondsCallback), waitForSeconds[currentTask]);
                    return;
                case TypeOfFunction.Condition:

                    if (!Networking.IsOwner(callback.gameObject)) Networking.SetOwner(player, callback.gameObject);

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
                if (!Networking.IsOwner(callbacks[currentTask].gameObject)) Networking.SetOwner(player, callbacks[currentTask].gameObject);
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


        public override void OnOwnershipTransferred(VRCPlayerApi _player)
        {
            taskCount = functions.Length - 1;
        }
    }
}