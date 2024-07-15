
using UdonSharp;
using UdonTasks.Runtime.Synced;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedTaskExample : UdonSharpBehaviour
{
    [UdonSynced] private int have;
    [UdonSynced] private int needed;

    public SyncedTask task;

    private VRCPlayerApi player;

    public void StartLogic()
    {
        player = Networking.LocalPlayer;

        if (!Utilities.IsValid(player)) return;

        if (!Networking.IsOwner(task.gameObject)) Networking.SetOwner(player, task.gameObject);
        task.StartTask();
    }

    public void Task1()
    {
        needed = Random.Range(0, 10);
        RequestSerialization();

        Debug.Log("Task 1 Completed");
    }

    public void Task2()
    {
        have = Random.Range(0, 10);
        RequestSerialization();

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
