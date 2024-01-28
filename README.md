# NTask

​	It provides some useful API's to help you to manager your routines. 

### WHY:

* `NTask` does not spend a frame when breaking
* We can pause \o/
* Can be moved step by step

​	The main difference between creating coroutines with `NTask` OR `StartCoroutine()` is the frames spend on `yield break;` and `yield return null`, when you use `StartCoroutine()` either instruction will spend 1 frame, so the following code should use 3 frames

```c#
private IEnumerator MyRoutine()
{
    yield return RoutineThatBreaks(); // 1 frame
    yield return null; // 1 frame
    yield return RoutineThatBreaks(); // 1 frame
}

private IEnumerator RoutineThatBreaks() { yield break; }
```

If you create the routine using `NTask` it will consume `yield break;` instructions, so the same code will take 1 frame only to finish

```c#
private IEnumerator MyRoutine()
{
    yield return RoutineThatBreaks(); // 0 frame
    yield return null; // 1 frame
    yield return RoutineThatBreaks(); // 0 frame
}

private IEnumerator RoutineThatBreaks() { yield break; }
```

Therefore is important to keep in mind that that you CANNOT PAUSE into `yield break` and also `MoveNext()` will not stop on it too, break instructions is like *"run this all on the same frame"*. 

**IMPORTANT:** Internally I'm using recursion by design, if not routines that loop through other routines that just breaks can freeze your code, take the following code as a sample, I'm exaggerating here but if all your routines breaks at some point your routine will just spend 1 frame on each when controlled by unity but it will thrown an *StackOverflow* when controlled by `Ntask`

```c#
private IEnumerator MyRoutine()
{
    while (true)
    {
        yield return RoutineThatBreaks();
        yield return RoutineThatBreaks();
    }
}
```

## How to use

​	The constructor has only two parameters, a required IEnumerator and an optional boolean to start right away or not

```c#
public NTask (IEnumerator initialRoutine, bool autoStart = true)
```

e.g.

```c#
...
    var myTask = new NTask(MyRoutine); // Will start run code right away
    var myPausedTask = new NTask(MyRoutine, false); // Will be on an idle state, an task.Start() need to be called
...
```

## Pause/Unpause

```c#
myTask.Pause(); // will halt it on the next yield
...
myTask.Unpause();
```

* Note that pausing/unpausing will work with any level of nested routines, you can check it on NTask sample scene
* Unpausing and pausing again on the same frame will not run one time, it will just keep paused

## Moving it step by step

​	Note that despite the name being equal to `IEnumerator.MoveNext()` this guy does not return a bool

```c#
myTask.MoveNext();
```

* It can be safely used with paused routines

* `MoveNext()` will by pass your `yield breaks`, so the following code will stop only on the `yield return null` and will execute any code inside `RoutineThatBreaks()` on the same frame

  ```c#
  private IEnumerator MyRoutine()
  {
      yield return RoutineThatBreaks();
      yield return RoutineThatBreaks();
      yield return null;
  }
  ```

## Yielding for NTask

​	On some occasions, like when having multiples tasks, to be able to yield for them, you can achieve it calling 

```c#
public void RunRoutine()
{
    StartCoroutine(WeCanYieldForTask());
}

private IEnumerator WeCanYieldForTask()
{
    taskA = new NTask(SomeOperation(), false);
    taskB = new NTask(SomeOtherOperation(), false);

    Debug.Log($"{Time.frameCount}");
    yield return taskA.GetEnumerator();
    Debug.Log($"{Time.frameCount}");
    yield return taskB.GetEnumerator();
    Debug.Log($"{Time.frameCount}");
}
```





## Know issues:

* Calling a `WaitForEndOfFrame` breaks the task, without any warning, it simple stop there




TODO FEAT:

- Some simple way to work with Service Locator + Lazy behavior + NULL pattern
- Cast items under mouse on scene, like PEEK does

TODO DOC:

- NTask
    - Wait for finish
    - ??Delay begin??
    - Beggining on next frame??
- NDictionary