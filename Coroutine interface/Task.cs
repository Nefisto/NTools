using System;
using System.Collections;

namespace NTools
{
    /// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
    /// It is an error to attempt to start a task that has been stopped or which has
    /// naturally terminated.
    public class Task
    {
        /// Returns true if and only if the coroutine is running.  Paused tasks
        /// are considered to be running.
        public bool Running => task.Running;

        /// Returns true if and only if the coroutine is currently paused.
        public bool Paused => task.Paused;

        /// Termination event.  Triggered when the coroutine completes execution.
        public event Action<bool> Finished;

        /// If autoStart is true (default) the task is automatically started upon construction.
        public Task (IEnumerator c, bool autoStart = true)
        {
            task = TaskManager.CreateTask(c);
            task.Finished += TaskFinished;

            if (autoStart)
                Start();
        }

        /// Begins execution of the coroutine
        public void Start (bool startOnNextFrame = false)
            => task.Start(startOnNextFrame);

        /// Discontinues execution of the coroutine at its next yield.
        public void Stop()
            => task.Stop();

        public void Pause()
            => task.Pause();

        public void Unpause()
            => task.Unpause();

        private void TaskFinished (bool manual)
        {
            var handler = Finished;
            if (handler != null)
                handler(manual);
        }

        private TaskManager.TaskState task;
    }
}