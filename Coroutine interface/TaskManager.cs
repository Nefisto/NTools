/// TaskManager.cs
/// Copyright (c) 2011, Ken Rockot  <k-e-n-@-REMOVE-CAPS-AND-HYPHENS-oz.gs>.  All rights reserved.
/// Everyone is granted non-exclusive license to do anything at all with this code.
///
/// This is a new coroutine interface for Unity.
///
/// The motivation for this is twofold:
///
/// 1. The existing coroutine API provides no means of stopping specific
///    coroutines; StopCoroutine only takes a string argument, and it stops
///    all coroutines started with that same string; there is no way to stop
///    coroutines which were started directly from an enumerator.  This is
///    not robust enough and is also probably pretty inefficient.
///
/// 2. StartCoroutine and friends are MonoBehaviour methods.  This means
///    that in order to start a coroutine, a user typically must have some
///    component reference handy.  There are legitimate cases where such a
///    constraint is inconvenient.  This implementation hides that
///    constraint from the user.
///
/// Example usage:
///
/// ----------------------------------------------------------------------------
/// IEnumerator MyAwesomeTask()
/// {
///     while(true) {
///         Debug.Log("Logcat iz in ur consolez, spammin u wif messagez.");
///         yield return null;
////    }
/// }
///
/// IEnumerator TaskKiller(float delay, Task t)
/// {
///     yield return new WaitForSeconds(delay);
///     t.Stop();
/// }
///
/// void SomeCodeThatCouldBeAnywhereInTheUniverse()
/// {
///     Task spam = new Task(MyAwesomeTask());
///     new Task(TaskKiller(5, spam));
/// }
/// ----------------------------------------------------------------------------
///
/// When SomeCodeThatCouldBeAnywhereInTheUniverse is called, the debug console
/// will be spammed with annoying messages for 5 seconds.
///
/// Simple, really.  There is no need to initialize or even refer to TaskManager.
/// When the first Task is created in an application, a "TaskManager" GameObject
/// will automatically be added to the scene root with the TaskManager component
/// attached.  This component will be responsible for dispatching all coroutines
/// behind the scenes.
///
/// Task also provides an event that is triggered when the coroutine exits.

// ! Got from: https://forum.unity.com/threads/a-more-flexible-coroutine-interface.94220/?_ga=2.56155371.1959154351.1600598637-1415149547.1596652085

using System;
using UnityEngine;
using System.Collections;

namespace NTools
{
    public class TaskManager : MonoBehaviour
    {
        public class TaskState
        {
            public event Action<bool> Finished;

            public bool Running { get; private set; }
            public bool Paused { get; private set; }

            private IEnumerator coroutine;
            private bool stopped;

            public TaskState (IEnumerator c)
                => coroutine = c;

            public void Pause()
                => Paused = true;

            public void Unpause()
                => Paused = false;

            public void Start (bool startOnNextFrame = false)
            {
                Running = true;
                singleton.StartCoroutine(CallWrapper(startOnNextFrame));
            }

            public void Stop()
            {
                stopped = true;
                Running = false;
            }

            private IEnumerator CallWrapper (bool startOnNextFrame = false)
            {
                var e = coroutine;

                if (startOnNextFrame)
                    yield return null;

                while (Running)
                {
                    if (Paused)
                        yield return null;
                    else
                    {
                        if (e != null && e.MoveNext())
                        {
                            yield return e.Current;
                        }
                        else
                        {
                            Running = false;
                        }
                    }
                }

                Finished?.Invoke(stopped);
            }
        }

        private static TaskManager singleton;

        public static TaskState CreateTask (IEnumerator coroutine)
        {
            if (singleton != null)
                return new TaskState(coroutine);

            var go = new GameObject("TaskManager");
            singleton = go.AddComponent<TaskManager>();
            return new TaskState(coroutine);
        }
    }
}
