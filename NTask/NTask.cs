using System;
using System.Collections;

namespace NTools
{
    public partial class NTask : IEnumerable
    {
        public bool IsRunning => task.IsRunning;
        private readonly NTaskManager.TaskState task;

        public NTask (IEnumerator initialRoutine, Settings settings = null)
        {
            settings ??= new Settings();
            
            task = NTaskManager.CreateTask(initialRoutine);
            task.OnFinished += TaskFinished;

            if (settings.autoStart)
                task.Start(settings);
        }

        public event Action<bool> OnFinished;

        private void TaskFinished (bool manual) => OnFinished?.Invoke(manual);

        public void Stop()
        {
            task.IsStopped = true;
            task.IsRunning = false;
        }

        public void Pause() => task.IsPaused = true;

        public void Resume()
        {
            if (!task.IsRunning)
            {
                task.IsPaused = false;
                task.Start();
                return;
            }
            
            task.IsPaused = false;
        }

        public void MoveNext() => task.MoveNext();

        public IEnumerator GetEnumerator()
        {
            yield return task.GetEnumerator();
        }

        public class Settings
        {
            public bool autoStart = true;
            public bool startOnNextFrame = false;
        }
    }
}