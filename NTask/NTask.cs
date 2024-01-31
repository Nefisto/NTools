using System;
using System.Collections;

namespace NTools
{
    public class NTask : IEnumerable
    {
        public bool IsRunning => task.IsRunning;
        private readonly NTaskManager.TaskState task;

        public NTask (IEnumerator initialRoutine, bool autoStart = true)
        {
            task = NTaskManager.CreateTask(initialRoutine);
            task.OnFinished += TaskFinished;

            if (autoStart)
                Start();
        }

        public void Start (bool startOnNextFrame = false) => task.Start(startOnNextFrame);

        public event Action<bool> OnFinished;

        private void TaskFinished (bool manual) => OnFinished?.Invoke(manual);

        public void Stop()
        {
            task.IsStopped = true;
            task.IsRunning = false;
        }

        public void Pause() => task.IsPaused = true;

        public void Unpause() => task.IsPaused = false;

        public void MoveNext() => task.MoveNext();

        public IEnumerator GetEnumerator()
        {
            yield return task.GetEnumerator();
        }
    }
}