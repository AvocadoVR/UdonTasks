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
    public readonly TypeOfFunction taskFunc = TypeOfFunction.Task;

    public int i;
    public bool manualConfirmation;
    public string methodName;
    public UdonBehaviour callback;
}

[Serializable]
public class Condition : IBaseType
{

    public readonly TypeOfFunction taskFunc = TypeOfFunction.Condition;

    public int i;
    public bool repeat;
    public string methodName;
    public UdonBehaviour callback;


}

[Serializable]
public class WaitForSeconds : IBaseType
{

    public readonly TypeOfFunction taskFunc = TypeOfFunction.WaitForSeconds;

    public int i;
    public int waitForSeconds;
}

