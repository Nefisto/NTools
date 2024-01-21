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

        public TaskState (IEnumerator initialRoutine)
        {
            initialRoutine.MoveNext();
            routinesStack.Push(initialRoutine);
        }

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

                while (e.Current is IEnumerator inner)
                {
                    // Inner routine has break
                    if (!inner.MoveNext())
                    {
                        if (!e.MoveNext())
                        {
                            _ = routinesStack.Pop();

                            if (routinesStack.Count == 0)
                            {
                                OnFinished?.Invoke(IsStopped);
                                yield break;
                            }
                            
                            e = routinesStack.Peek();
                        }

                        continue;
                    }

                    routinesStack.Push(inner);
                    e = inner;
                }

                do
                {
                    if (e.Current is IEnumerator)
                        break;

                    yield return e.Current;

                    while (IsPaused)
                        yield return null;
                } while (e.MoveNext());

                _ = routinesStack.Pop();
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

                    // Routine breaks
                    if (!e.MoveNext())
                        e = routinesStack.Pop();
                }
            }
            else
            {
                routinesStack.Pop();
                MoveNext();
            }
        }

        private bool TryMoveNext (IEnumerator routine, out bool hasMoved)
        {
            hasMoved = routine.MoveNext();
            return hasMoved;
        }
    }
}