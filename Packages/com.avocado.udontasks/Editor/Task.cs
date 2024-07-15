using System;
using System.Collections;
using System.Collections.Generic;
using UdonTasks.Editor;
using UdonTasks.Runtime;
using UnityEngine;
using VRC.Udon;

[Serializable]
public class Task : IBaseType
{
    public int i;
    public string methodName;
    public UdonBehaviour methodBehavior;
    public bool manualConfirmation;

    public readonly TypeOfFunction taskFunc = TypeOfFunction.Task;
}

[Serializable]
public class Condition : IBaseType
{
    public int i;
    public bool repeat;
    public string conditionMethod;
    public UdonBehaviour callback;

    public readonly TypeOfFunction taskFunc = TypeOfFunction.Condition;

}

[Serializable]
public class WaitForSeconds : IBaseType
{
    public int i;
    public int waitForSeconds;

    public readonly TypeOfFunction taskFunc = TypeOfFunction.WaitForSeconds;
}

