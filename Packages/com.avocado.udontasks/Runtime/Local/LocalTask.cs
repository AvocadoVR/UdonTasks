
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
        private bool ConditionMet = false;
        public bool isTaskRunning { get; private set; } = false;

        #region Set Code
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

        private int currentState = -1;
        private int stateCount;

        public string[] methodNames;
        public bool[] manualConfirmation;
        public UdonBehaviour[] methodBehaviors;

        public TypeOfFunction[] function;

        public string[] conditionMethods;
        public bool[] repeatUntilMet;
        public UdonBehaviour[] callbacks;

        public int[] waitForSeconds;

        void Start()
        {
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
            if (!isTaskRunning) return;

            currentState++;

            switch (function[currentState])
            {
                case TypeOfFunction.Task:
                    if (methodBehaviors[currentState] == null || string.IsNullOrEmpty(methodNames[currentState]) || string.IsNullOrWhiteSpace(methodNames[currentState]))
                    {
                        StopTask();
                        return;
                    }

                    methodBehaviors[currentState].SendCustomEvent(methodNames[currentState]);

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
                    if (callbacks[currentState] == null || string.IsNullOrEmpty(conditionMethods[currentState]) || string.IsNullOrWhiteSpace(conditionMethods[currentState]))
                    {
                        StopTask();
                        return;
                    }

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
    }
}