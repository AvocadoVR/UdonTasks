# UdonTasks

# Support
If you would like to support me & my work. Consider supporting me on [Patreon](https://www.patreon.com/AvocadoVR)

## Local Task & Synced Task

[Local Task Example]() | [Synced Task Example]()

>| Methods | Description | Usage |
>|---|---|---|
>| StartTask() | This starts the task. |  |
>| StopTask() | This stops the task. |  |
>| Continue() | Conditional: If your task is marked as Manual Confirmation. You will need to use this after your logic in your callback. | If you want something to execute a task and make sure the data has time to update. |
>| SetCondition(bool Condition) | Conditional: Whether you mark it as Repeat til Fulfillment or not. This will be needed to tell the Task how to proceed.<br><br>True: It met the condition.<br><br>False: It didn't meet the condition. | If you have a task that shouldn't execute unless these conditions are met.  |
>| WaitForSecondsCallback() | DO NOT USE! This function is for the task script only. |  |

<br>

>| Variable           | Type             | Description                                                               | Access Type |
>|--------------------|------------------|---------------------------------------------------------------------------|-------------|
>| isConditionMet     | bool             | Used when there is a Conditional function.                                | private     |
>| isTaskRunning      | bool             | Returns the state of the task.                                            | public      |
>| currentTask        | int              | Which task its currently on.                                              | private     |
>| taskCount          | int              | How many tasks in total.                                                  | private     |
>| functions          | TypeOfFunction[] | Holds the steps of your task layout.                                      | public      |
>| methodNames        | string[]         | Holds names of the methods.                                               | public      |
>| callbacks          | UdonBehaviour[]  | Holds all the UdonBehaviours.                                             | public      |
>| manualConfirmation | bool[]           | Used for tasks that want to be manually confirmed.                        | public      |
>| repeatUntilMet     | bool[]           | Used for Condition task that want to repeat until it meets the Condition. | public      |
>| waitForSeconds     | int[]            | Holds all WaitForSeconds integers.                                        | public      |

<br>
<br>
<br>

## Local Task & Synced Task Base

[Local Task Base Example]() | [Synced Task Base Example]()

>| Methods                  | Description                                                       |
>|--------------------------|-------------------------------------------------------------------|
>| StartTask()              | Starts the task.                                                  |
>| StopTask()               | Stops the task.                                                   |
>| Task()                   | This is for your logic.                                           |
>| Continue()               | This will allow the task to continue after triggering your logic. |
>| WaitForSeconds()         | Wait a period of time before trigger your next task.              |
>| WaitForSecondsCallback() | DO NOT USE! This function is for the task script only.            |

<br>

>| Variable      | Type | Description                                               | Access Type |
>|---------------|------|-----------------------------------------------------------|-------------|
>| currentTask   | int  | Shows the current task its on.                            | public      |
>| taskAmount    | int  | Show how many task in total todo.                         | public      |
>| blockContinue | bool | Internal Logic to prevent WaitForSeconds for not working. | private     |
>| isTaskRunning | bool | Returns the state of the task.                            | public      |

<br>
<br>
<br>

## Local Timer & Synced Timer Task

[Local Timer Task Example]() | [Synced Timer Task Example]()

>| Method                       | Description                                           |
>|------------------------------|-------------------------------------------------------|
>| SetTimer(int _min, int _sec) | Used to set the timer. Use before starting the timer! |
>| StartTimer()                 | Starts the timer.                                     |
>| StopTimer()                  | Stops the timer.                                      |
>| Timer()                      | Timer Logic.                                          |
>| OnTimerTick()                | Everytime the timer triggers.                         |
>| OnTimerCompletion()          | Triggers when the timer is done.                      |
>| OnTimerStop()                | Triggers when the timer stops.                        |

<br>

>| Variable      | Type | Description                           | Access Type      |
>|---------------|------|---------------------------------------|------------------|
>| minutes       | int  | Shows how many minutes remaining.     | public & private |
>| seconds       | int  | Shows how many seconds are remaining. | public & private |
>| isTaskRunning | bool | Returns the state of the task.        | public           |