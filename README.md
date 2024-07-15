# UdonTasks

# Support
If you would like to support me & my work. Consider supporting me on [Patreon](https://www.patreon.com/AvocadoVR)

## Local Task & Synced Task Methods
| Methods | Description | Usage |
|---|---|---|
| StartTask() | This starts the task. |  |
| StopTask() | This stops the task. |  |
| Continue() | Conditional: If your task is marked as Manual Confirmation. You will need to use this after your logic in your callback. | If you want something to execute a task and make sure the data has time to update. |
| SetCondition(bool Condition) | Conditional: Whether you mark it as Repeat til Fulfillment or not. This will be needed to tell the Task how to proceed.<br><br>True: It met the condition.<br><br>False: It didn't meet the condition. | If you have a task that shouldn't execute unless these conditions are met.  |
| WaitForSecondsCallback() | DO NOT USE! This function is for the task script only. |  |

<br>

## Local Task & Synced Task Variables (Internal/External)
| Variable           | Type             | Description                                                                  | Access Type  |
|--------------------|------------------|------------------------------------------------------------------------------|--------------|
| ConditionMet       | bool             | Used when there is a Conditional function.                                   | Internal     |
| isTaskRunning      | bool             | Returns the state of the task.                                               | **Internal/External** |
| currentState       | int              | Which function its on.                                                       | Internal     |
| stateCount         | int              | Total amount of functions.                                                   | Internal     |
| methodNames        | string[]         | Holds all the task method names.                                             | Internal     |
| manualConfirmation | bool[]           | Used for tasks that want to be manually confirmed.                           | Internal     |
| methodBehaviors    | UdonBehaviour[]  | Holds all the Method Behaviours                                              | Internal     |
| function           | TypeOfFunction[] | Holds the steps of your task layout.                                         | Internal     |
| conditionMethods   | string[]         | Holds all conditional method names.                                          | Internal     |
| repeatUntilMet     | bool[]           | Used for Condition Scripts that want to repeat until it meets the Condition. | Internal     |
| callbacks          | UdonBehaviour[]  | Holds all the Conditional UdonBehaviours.                                    | Internal     |
| waitForSeconds     | int[]            | Holds all WaitForSeconds function data.                                      | Internal     |
