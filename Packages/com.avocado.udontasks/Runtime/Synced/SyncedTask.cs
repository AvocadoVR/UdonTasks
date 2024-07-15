
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine;

namespace UdonTasks.Runtime.Synced
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncedTask : UdonSharpBehaviour
    {
        private bool ConditionMet = false;

        [UdonSynced] public bool isTaskRunning = false;

        #region Set Code
        public bool IsTaskRunning()
        {
            return isTaskRunning;
        }

        public void SetCondition(bool Condition)
        {
            if (!repeatUntilMet[currentState])
            {
                ConditionMet = Condition;
                ConditonalBreak();
            }
            else
            {
                ConditionMet = Condition;
                ConditonalRepeat();
            }
        }
        #endregion

        [UdonSynced] private int currentState = -1;
        private int stateCount;

        public string[] methodNames;
        public bool[] manualConfirmation;
        public UdonBehaviour[] methodBehaviors;

        public TypeOfFunction[] function;

        public string[] conditionMethods;
        public bool[] repeatUntilMet;
        public UdonBehaviour[] callbacks;

        public int[] waitForSeconds;

        private VRCPlayerApi player;

        void Start()
        {
            player = Networking.LocalPlayer;
            stateCount = function.Length - 1;
        }

        public void StartTask()
        {
            isTaskRunning = true;
            TaskFunc();
        }

        public void StopTask()
        {
            isTaskRunning = false;
            currentState = -1;
            ConditionMet = false;
        }

        private void TaskFunc()
        {
            if (!isTaskRunning || !Utilities.IsValid(player))
            {
                StopTask();
                return;
            }

            currentState++;

            switch (function[currentState])
            {
                case TypeOfFunction.Task:
                    var behaviour = methodBehaviors[currentState];

                    if (behaviour == null || string.IsNullOrEmpty(methodNames[currentState]) || string.IsNullOrWhiteSpace(methodNames[currentState]))
                    {
                        StopTask();
                        return;
                    }

                    if (!Networking.IsOwner(behaviour.gameObject)) Networking.SetOwner(player, behaviour.gameObject);

                    behaviour.SendCustomEvent(methodNames[currentState]);

                    if (manualConfirmation[currentState]) return;
                    break;
                case TypeOfFunction.WaitForSeconds:
                    if (currentState == stateCount)
                    {
                        StopTask();
                        return;
                    }

                    SendCustomEventDelayedSeconds(nameof(WaitForSecondsCallback), waitForSeconds[currentState]);
                    return;
                case TypeOfFunction.Condition:
                    var callback = callbacks[currentState];
                    if (callback == null || string.IsNullOrEmpty(conditionMethods[currentState]) || string.IsNullOrWhiteSpace(conditionMethods[currentState]))
                    {
                        StopTask();
                        return;
                    }

                    if (!Networking.IsOwner(callback.gameObject)) Networking.SetOwner(player, callback.gameObject);

                    callbacks[currentState].SendCustomEvent(conditionMethods[currentState]);
                    return;
            }


            if (currentState < stateCount)
            {
                TaskFunc();
            }
            else if (currentState == stateCount)
            {
                StopTask();
            }
        }

        public void Continue()
        {
            if (!manualConfirmation[currentState] || currentState == stateCount || !isTaskRunning)
            {
                StopTask();
                return;
            }

            TaskFunc();
        }


        private void ConditonalBreak()
        {
            if (ConditionMet || currentState == stateCount || function[currentState] != TypeOfFunction.Condition || !isTaskRunning) // Prevents people from causing errors.
            {
                StopTask();
                return;
            }

            TaskFunc();
        }

        private void ConditonalRepeat()
        {
            if (currentState == stateCount || function[currentState] != TypeOfFunction.Condition || !isTaskRunning)
            {
                StopTask();
                return;
            }

            if (!ConditionMet)
            {
                if (!Networking.IsOwner(callbacks[currentState].gameObject)) Networking.SetOwner(player, callbacks[currentState].gameObject);
                callbacks[currentState].SendCustomEvent(conditionMethods[currentState]);
            }
            else if (ConditionMet)
            {
                TaskFunc();
            }
        }

        public void WaitForSecondsCallback()
        {
            if (!isTaskRunning || function[currentState] != TypeOfFunction.WaitForSeconds) return;

            TaskFunc();
        }


        public override void OnOwnershipTransferred(VRCPlayerApi _player)
        {
            stateCount = function.Length - 1;
        }
    }
}