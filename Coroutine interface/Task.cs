using System;
using System.Collections;

public class Task
{
    private bool isPaused;
    private bool isStopped;
    private readonly TaskManager.TaskState task;

    public Task (IEnumerator initialRoutine, bool autoStart = true)
    {
        task = TaskManager.CreateTask(initialRoutine);
        task.OnFinished += TaskFinished;
        
        if (autoStart)
            Start();
    }

    public void Start (bool startOnNextFrame = false)
        => task.Start(startOnNextFrame);

    public event Action<bool> OnFinished;

    private void TaskFinished (bool manual)
        => OnFinished?.Invoke(manual);

    public void Stop()
    {
        task.IsStopped = true;
        task.IsRunning = false;
    }

    public void Pause()
        => task.IsPaused = true;

    public void Unpause()
        => task.IsPaused = false;

    public void MoveNext() => task.MoveNext();
}