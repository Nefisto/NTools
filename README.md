TODO FEAT:
- Some simple way to work with Service Locator + Lazy behavior + NULL pattern

TODO DOC:

# NTask

​	It provides some useful API's to help you to manager your routines

## How to use

```c#
var myTask = new NTask(MyRoutine);
```



## Pause/Unpause

```c#
myTask.Pause(); // will halt it on the next yield
...
myTask.Unpause();
```

Note that pausing/unpausing will work with any level of nested routines, you can check it on NTask sample scene



- NTask
    - Calling routine step-by-step with Move next
    - Yield break not use a frame
    - Yield return skip one frame
    - ??Delay begin??
    - Beggining on next frame??
- NDictionary