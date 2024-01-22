# NTask

​	It provides some useful API's to help you to manager your routines

## How to use

```c#
...
    var myTask = new NTask(MyRoutine);
...
```

The main difference between starting coroutines with `NTask` OR `StartCoroutine()` is the frames spend on `yield break;` and `yield return null`, when you use `StartCoroutine()` either instruction will spend 1 frame, so the following code should use 3 frames

```c#
private IEnumerator MyRoutine()
{
    yield return RoutineThatBreaks(); // 1 frame
    yield return null; // 1 frame
    yield return RoutineThatBreaks(); // 1 frame
}

private IEnumerator RoutineThatBreaks() { yield break; }
```

But when `NTask` is managing your routine it will bypass `yield break;` instructions, so the same code should use 1 frame only, therefore is important to keep in mind that that you CANNOT PAUSE into `yield break` and also `MoveNext()` will not stop on it too, break instructions is like "run this all on the same frame". 

IMPORTANT: This kind of operation can be desired on some cases but it can also cause problem on others, take the following code as a sample, I'm exaggerating here but if all your routines breaks at some point your routine will just spend 1 frame on each when controlled by unity but it will thrown an *StackOverflow* when controlled by `Ntask`, as you can imagine, its works exactly like a loop without escape in a synchronized code

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



## Pause/Unpause

```c#
myTask.Pause(); // will halt it on the next yield
...
myTask.Unpause();
```

* Note that pausing/unpausing will work with any level of nested routines, you can check it on NTask sample scene
* Unpausing and pausing again on the same frame will not run one time, it will just keep paused

## Moving it step by step

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

  

TODO FEAT:

- Some simple way to work with Service Locator + Lazy behavior + NULL pattern

TODO DOC:



- NTask
    - ??Delay begin??
    - Beggining on next frame??
    - Yielding for the NTask
    - What is the behavior of Unpausing/Pausing on the same frame?
- NDictionary