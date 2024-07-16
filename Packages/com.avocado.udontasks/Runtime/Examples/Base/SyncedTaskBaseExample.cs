
using UdonSharp;
using UdonTasks.Runtime.Synced;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedTaskBaseExample : SyncedTaskBase
{
    /// <summary>
    /// (UdonSynced) currentTask - What task it is currently on.
    /// taskAmount - How many tasks in total. You can do an array index like 0 and up or just start with 1. Just make sure you set it in the unity inspector.
    /// isTaskRunning - Returns the state of the task
    /// 
    /// You can override all methods from LocalTaskBase
    /// 
    /// StartTask() - Starts the task. 
    /// StopTask() - Stops the task.
    /// Task() - Your Code.
    /// Continue() - Make it execute the next task.
    /// WaitForSeconds(int seconds) - Sends a delayed event of WaitForSecondsCallback().
    /// WaitForSecondsCallback() - Is called by WaitForSeconds and then triggers Task() so you get the delay.
    /// </summary>

    void Start()
    {
        StartTask();
    }

    public override void Task()
    {
        // Use a switch statement to have it easily execute your tasks.
        switch (currentTask)
        {
            case 0:
                Debug.Log("Working - WaitForSeconds(6)");
                WaitForSeconds(6);
                break;
            case 1:
                Debug.Log("Working - Task 1");
                break;
            case 2:
                Debug.Log("Working - WaitForSeconds(5)");
                WaitForSeconds(5);
                break;
            case 3:
                Debug.Log("Working - Task 2");
                break;
            case 4:
                int randomNumber = Random.Range(0, 5);

                if (randomNumber > 4)
                {
                    Debug.Log($"{randomNumber} > 4");
                }
                else
                {
                    Debug.Log($"{randomNumber} < 4");
                }
                break;
            case 5:
                Debug.Log("Working - Task 5");
                break;
        }

        Continue(); // Informs the task it has executed the code.      
    }
}
