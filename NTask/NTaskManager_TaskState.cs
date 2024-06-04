using System;
using System.Collections;
using System.Collections.Generic;

namespace NTools
{
    public partial class NTaskManager
    {
        public class TaskState : IEnumerable
        {
            private readonly Stack<IEnumerator> routinesStack = new();

            public TaskState (IEnumerator initialRoutine) => routinesStack.Push(initialRoutine);

            public bool IsRunning { get; set; }
            public bool IsPaused { get; set; }
            public bool IsStopped { get; set; }

            public IEnumerator GetEnumerator()
            {
                while (true)
                {
                    if (routinesStack.Count == 0)
                        yield break;

                    var e = routinesStack.Peek();
                    if (e != null && e.MoveNext())
                    {
                        while (e.Current is IEnumerator current)
                        {
                            routinesStack.Push(current);
                            e = current;
                            e.MoveNext();
                        }

                        yield return e.Current;
                    }
                    else
                    {
                        routinesStack.Pop();
                    }
                }
            }

            public event Action<bool> OnFinished;

            public void Start (NTask.Settings settings = null)
            {
                settings ??= new NTask.Settings();

                IsRunning = true;
                singleton.StartCoroutine(Wrapper(settings));
            }

            private IEnumerator Wrapper (NTask.Settings settings)
            {
                if (settings.startOnNextFrame)
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

                EndTask();
            }

            public void MoveNext()
            {
                if (routinesStack.Count == 0)
                {
                    EndTask();
                    return;
                }

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

            private void EndTask()
            {
                IsRunning = false;
                OnFinished?.Invoke(IsStopped);
            }

            [Obsolete("Use version with settings")]
            public void Start (bool startOnNextFrame)
            {
                IsRunning = true;
                singleton.StartCoroutine(Wrapper(new NTask.Settings { startOnNextFrame = startOnNextFrame }));
            }
        }
    }
}