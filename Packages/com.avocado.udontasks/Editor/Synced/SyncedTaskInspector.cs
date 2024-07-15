using System.Collections;
using System.Collections.Generic;
using UdonTasks.Runtime;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VRC.Udon;
using UdonTasks.Runtime.Synced;

namespace UdonTasks.Editor.Synced
{
    [CustomEditor(typeof(SyncedTask))]
    public class SyncedTaskInspector : UnityEditor.Editor
    {
        private List<IBaseType> elements = new List<IBaseType>();

        private List<bool> foldout = new List<bool>();

        private ReorderableList list;

        void OnEnable()
        {
            Load();

            list = new ReorderableList(elements, typeof(IBaseType), true, true, true, true);

            list.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Tasks");
            };

            list.elementHeightCallback = (index) =>
            {
                var element = elements[index];

                if (element is Task task)
                {
                    return 82;
                }
                else if (element is Condition condition)
                {
                    return 82;
                }
                else if (element is WaitForSeconds wait)
                {
                    return 20;
                }

                return 0;
            };


            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 2;
                var element = elements[index];

                EditorGUI.BeginChangeCheck();

                float lineHeight = EditorGUIUtility.singleLineHeight;
                float padding = 4f;
                float foldoutSize = 12f;
                float iconSize = 14f;

                float totalHeight = 0f;

                float foldoutWidth = foldoutSize + 10f; // Adjust as needed

                int space = 20;


                if (element is Task || element is Condition)
                {
                    totalHeight = EditorGUIUtility.singleLineHeight * 5 + padding * 3;

                    rect.width -= iconSize;
                    rect.width += iconSize;

                    rect.x += iconSize;
                    foldout[index] = EditorGUI.Foldout(new Rect(rect.x, rect.y, foldoutSize, lineHeight), foldout[index], GUIContent.none);

                    // Draw label indicating the type (Task or Condition)
                    string elementTypeLabel = "";
                    if (element is Task)
                    {
                        Task task = element as Task;

                        if (task.methodName == "" || string.IsNullOrWhiteSpace(task.methodName))
                        {
                            elementTypeLabel = "Task";
                        }
                        else elementTypeLabel = $"Task - {task.methodName}";

                    }
                    else
                    {
                        Condition condition = element as Condition;

                        if (condition.conditionMethod == "" || string.IsNullOrWhiteSpace(condition.conditionMethod))
                        {
                            elementTypeLabel = "Condition";
                        }
                        else elementTypeLabel = $"Condition - {condition.conditionMethod}";
                    }

                    EditorGUI.LabelField(new Rect(rect.x + foldoutSize - 8f, rect.y, rect.width - foldoutWidth, lineHeight), elementTypeLabel, EditorStyles.boldLabel);

                    if (foldout[index])
                    {
                        EditorGUI.indentLevel++;
                        rect.x += foldoutSize;

                        rect.y += lineHeight; // Move down below the foldout arrow and label

                        if (element is Task)
                        {
                            Task task = (Task)element;
                            task.i = index;
                            task.manualConfirmation = EditorGUI.Toggle(new Rect(rect.x - space, rect.y, rect.width - 50, lineHeight), "Manual Confirmation", task.manualConfirmation);
                            rect.y += lineHeight + padding;
                            task.methodName = EditorGUI.TextField(new Rect(rect.x - space, rect.y, rect.width - 50, lineHeight), "Method", task.methodName);
                            rect.y += lineHeight + padding;
                            task.methodBehavior = (UdonBehaviour)EditorGUI.ObjectField(new Rect(rect.x - space, rect.y, rect.width - 50, lineHeight), "Script", task.methodBehavior, typeof(UdonBehaviour), true);
                        }
                        else if (element is Condition)
                        {
                            Condition condition = (Condition)element;
                            condition.i = index;
                            condition.repeat = EditorGUI.Toggle(new Rect(rect.x - space, rect.y, rect.width - 50, lineHeight), "Repeat Until Fulfillment", condition.repeat);
                            rect.y += lineHeight + padding;
                            condition.conditionMethod = EditorGUI.TextField(new Rect(rect.x - space, rect.y, rect.width - 50, lineHeight), "Method", condition.conditionMethod);
                            rect.y += lineHeight + padding;
                            condition.callback = (UdonBehaviour)EditorGUI.ObjectField(new Rect(rect.x - space, rect.y, rect.width - 50, lineHeight), "Callback", condition.callback, typeof(UdonBehaviour), true);
                        }

                        rect.x -= foldoutSize;
                        EditorGUI.indentLevel--;
                    }
                }
                else if (element is WaitForSeconds)
                {
                    totalHeight = EditorGUIUtility.singleLineHeight * 2 + padding * 1;

                    WaitForSeconds wait = (WaitForSeconds)element;
                    wait.i = index;

                    GUIStyle style = new GUIStyle(EditorStyles.numberField);
                    style.fixedWidth = 40;

                    wait.waitForSeconds = EditorGUI.IntField(new Rect(rect.x, rect.y, rect.width, lineHeight), "Wait For Seconds", wait.waitForSeconds, style);
                    rect.y += lineHeight;
                }

                rect.height = Mathf.Max(rect.height, rect.y - rect.yMin + lineHeight);

                if (EditorGUI.EndChangeCheck())
                {
                    elements[index] = element;
                    Save();
                }
            };



            list.onReorderCallback = (ReorderableList l) =>
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];

                    if (element is Task task)
                    {
                        task.i = i;
                        elements[i] = task;
                    }
                    else if (element is Condition condition)
                    {
                        condition.i = i;
                        elements[i] = condition;
                    }
                    else if (element is WaitForSeconds wait)
                    {
                        wait.i = i;
                        elements[i] = wait;
                    }
                }

                Save();
            };
            list.onRemoveCallback = (ReorderableList l) =>
            {
                if (elements.Count > 0 && l.index >= 0 && l.index < elements.Count)
                {
                    elements.RemoveAt(l.index);

                    for (int i = 0; i < elements.Count; i++)
                    {
                        var element = elements[i];

                        if (element is Task task)
                        {
                            task.i = i;
                        }
                        else if (element is Condition condition)
                        {
                            condition.i = i;
                        }
                        else if (element is WaitForSeconds wait)
                        {
                            wait.i = i;
                        }
                    }
                    Save();
                }
            };


            list.onAddDropdownCallback = (rect, list) =>
            {
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Task"), false, () =>
                {
                    elements.Add(new Task());
                    foldout.Add(false);
                    Save();
                });
                menu.AddItem(new GUIContent("Condition"), false, () =>
                {
                    elements.Add(new Condition());
                    foldout.Add(false);
                    Save();
                });
                menu.AddItem(new GUIContent("WaitForSeconds"), false, () =>
                {
                    elements.Add(new WaitForSeconds());
                    foldout.Add(false);
                    Save();
                });

                menu.ShowAsContext();
            };
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            list.DoLayoutList();

            if (EditorGUI.EndChangeCheck())
            {
                Save();
            }
        }

        public void Load()
        {
            SyncedTask inspector = (SyncedTask)target;

            if (inspector.function == null) return;

            for (int i = 0; i < inspector.function.Length; i++)
            {
                foldout.Add(false);
            }

            string[] md = inspector.methodNames;
            bool[] mc = inspector.manualConfirmation;
            UdonBehaviour[] mb = inspector.methodBehaviors;
            string[] cm = inspector.conditionMethods;
            bool[] rum = inspector.repeatUntilMet;
            UdonBehaviour[] cb = inspector.callbacks;
            int[] ws = inspector.waitForSeconds;
            TypeOfFunction[] func = inspector.function;

            for (int i = 0; i < func.Length; i++)
            {
                switch (func[i])
                {
                    case TypeOfFunction.Task:
                        Task task = new Task();
                        task.i = i;
                        task.methodName = md[i];
                        task.manualConfirmation = mc[i];
                        task.methodBehavior = mb[i];
                        elements.Add(task);
                        break;
                    case TypeOfFunction.WaitForSeconds:
                        WaitForSeconds wait = new WaitForSeconds();
                        wait.i = i;
                        wait.waitForSeconds = ws[i];
                        elements.Add(wait);
                        break;
                    case TypeOfFunction.Condition:
                        Condition condition = new Condition();
                        condition.i = i;
                        condition.conditionMethod = cm[i];
                        condition.repeat = rum[i];
                        condition.callback = cb[i];
                        elements.Add(condition);
                        break;
                }
            }
        }


        public void Save()
        {
            SyncedTask inspector = (SyncedTask)target;
            int length = elements.Count;


            inspector.methodNames = new string[length];
            inspector.manualConfirmation = new bool[length];
            inspector.methodBehaviors = new UdonBehaviour[length];
            inspector.conditionMethods = new string[length];
            inspector.repeatUntilMet = new bool[length];
            inspector.callbacks = new UdonBehaviour[length];
            inspector.waitForSeconds = new int[length];
            inspector.function = new TypeOfFunction[length];

            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];

                if (element is Task task)
                {
                    if (task.i >= 0 && task.i < length)
                    {
                        inspector.manualConfirmation[task.i] = task.manualConfirmation;
                        inspector.methodNames[task.i] = task.methodName;
                        inspector.methodBehaviors[task.i] = task.methodBehavior;
                        inspector.function[task.i] = task.taskFunc;
                    }
                }
                else if (element is Condition condition)
                {
                    if (condition.i >= 0 && condition.i < length)
                    {
                        inspector.repeatUntilMet[condition.i] = condition.repeat;
                        inspector.conditionMethods[condition.i] = condition.conditionMethod;
                        inspector.callbacks[condition.i] = condition.callback;
                        inspector.function[condition.i] = condition.taskFunc;
                    }
                }
                else if (element is WaitForSeconds wait)
                {
                    if (wait.i >= 0 && wait.i < length)
                    {
                        inspector.waitForSeconds[wait.i] = wait.waitForSeconds;
                        inspector.function[wait.i] = wait.taskFunc;
                    }
                }
            }
        }
    }
}
