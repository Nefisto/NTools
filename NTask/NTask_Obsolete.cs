using System;
using System.Collections;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace NTools
{
    public partial class NTask
    {
        [Obsolete("Use the version with a settings instead")]
        public NTask (IEnumerator initialRoutine, bool autoStart)
        {
            task = NTaskManager.CreateTask(initialRoutine);
            task.OnFinished += TaskFinished;

            if (autoStart)
                Start();
        }

        [Obsolete("Use Resume instead")]
        public void Unpause() => task.IsPaused = false;

        [Obsolete("Tasks that didn't autoStart now is started as paused, so we don't call Start manually anymore, just resume")]
        public void Start (bool startOnNextFrame = false) => task.Start(startOnNextFrame);
    }
}