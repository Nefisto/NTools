// ! Original code from here: https://forum.unity.com/threads/a-more-flexible-coroutine-interface.94220/?_ga=2.56155371.1959154351.1600598637-1415149547.1596652085

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTaskManager : MonoBehaviour
{
    private static NTaskManager singleton;

    public static TaskState CreateTask (IEnumerator routine)
    {
        if (singleton != null)
            return new TaskState(routine);

        singleton = new GameObject("Task manager").AddComponent<NTaskManager>();
        return new TaskState(routine);
    }
    
    public class TaskState
    {
        public event Action<bool> OnFinished;
        
        private readonly Stack<IEnumerator> routinesStack = new();

        public TaskState (IEnumerator initialRoutine) => routinesStack.Push(initialRoutine);

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

            var e = routinesStack.Peek();
            while (IsRunning)
            {
                if (e.MoveNext())
                {
                    if (e.Current is IEnumerator inner)
                    {
                        routinesStack.Push(inner);
                        e = routinesStack.Peek();
                        continue;
                    }
                    
                    yield return e.Current;
                    
                    while (IsPaused)
                        yield return null;

                    // We could have moved the routine through MoveNext, so a Peek is necessary
                    e = routinesStack.Peek();
                    
                    continue;
                }

                routinesStack.Pop();
                if (routinesStack.Count == 0)
                    break;

                e = routinesStack.Peek();
            }

            OnFinished?.Invoke(IsStopped);
        }

        public void MoveNext()
        {
            if (routinesStack.Count == 0)
                return;
            
            var e = routinesStack.Peek();
            if (e != null && e.MoveNext())
            {
                while (e.Current is IEnumerator current)
                {
                    routinesStack.Push(current);
                    e = current;
                    MoveNext();
                }
            }
            else
            {
                routinesStack.Pop();
                MoveNext();
            }
        }
    }
}