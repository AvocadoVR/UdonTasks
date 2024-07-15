
using UdonSharp;
using UdonTasks.Runtime.Local;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LocalTaskExample : UdonSharpBehaviour
{
    private int have;
    private int needed;

    public LocalTask task;

    public void StartLogic()
    {
        task.StartTask();
    }

    public void Task1()
    {
        needed = Random.Range(0, 10);
        Debug.Log("Task 1 Completed");
    }

    public void Task2()
    {
        have = Random.Range(0, 10);
        Debug.Log("Task 2 Completed");
    }

    public void ConditionCheck()
    {
        if (have == needed)
        {
            task.SetCondition(true);
            Debug.Log($"Have: {have} : Needed: {needed}");
        }
        else task.SetCondition(false);



    }
}
