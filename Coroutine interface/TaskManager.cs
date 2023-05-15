// ! Original code from here: https://forum.unity.com/threads/a-more-flexible-coroutine-interface.94220/?_ga=2.56155371.1959154351.1600598637-1415149547.1596652085

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private static TaskManager singleton;

    public static TaskState CreateTask (IEnumerator routine)
    {
        if (singleton != null)
            return new TaskState(routine);

        singleton = new GameObject("Task manager").AddComponent<TaskManager>();
        return new TaskState(routine);
    }
    
    public class TaskState
    {
        public event Action<bool> OnFinished;
        
        private readonly Stack<IEnumerator> routinesStack = new();

        public TaskState (IEnumerator initialRoutine)
            => routinesStack.Push(initialRoutine);

        public bool IsRunning { get; set; }
        public bool IsPaused { get; set; }
        public bool IsStopped { get; set; }

        public void Start (bool startOnNextFrame)
        {
            IsRunning = true;
            singleton.StartCoroutine(Wrapper(startOnNextFrame));
        }

        private IEnumerator Wrapper (bool startOnNextFrame = false)
        {
            if (startOnNextFrame)
                yield return null;

            while (IsRunning)
            {
                if (routinesStack.Count == 0)
                {
                    IsRunning = false;
                    break;
                }

                var e = routinesStack.Peek();

                if (IsPaused)
                {
                    yield return null;
                    continue;
                }

                if (e != null && e.MoveNext())
                {
                    while (e.Current is IEnumerator current)
                    {
                        routinesStack.Push(current);
                        e = routinesStack.Peek();
                    }

                    yield return e.Current;
                }
                else
                {
                    e = routinesStack.Pop();
                    yield return e.Current;
                }
            }

            OnFinished?.Invoke(IsStopped);
        }
    }
}