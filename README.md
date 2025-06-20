# Summary

- [Entry point](#EntryPoint)
- [Service locator](#service-locator)
- Blocker
- NTask
- Useful components
- Extensions

# ENTRY POINTS ASYNC <a name="EntryPoint">

​	Allow inject async code from external source and taking control of the flow. They are following observer terminology suggestion using caller and `EventArgs` as parameter

### WHY:

* We commonly need to make an external routine to run before something happens on an already existing executing flow. **e.g.** an status effect feedback should run every time that it ticks before the battle continue

### **HOW  TO:**

​	You need to create the entry points on your flow. I'm simulating an snippet on some kind of turn based RPG where the context of current action is passed through the battle flow

```c#
public class DamageContext : EventArgs
{
    public int amount;
}

public EntryPointAsync TakingDamageEntryPoint { get; set; } = new();
public EntryPointAsync TookDamageEntryPoint { get; set; } = new();

private async UniTask MyBattleRoutine()
{
    var damageContext = new DamageContext() { amount = 1 };

    await TakingDamageEntryPoint.InvokeAsync(this, damageContext);
    // Take dame logic
    await TookDamageEntryPoint.InvokeAsync(this, damageContext);
}
```

​	With these events (`TakingDamageEntryPoint` and `TookDamageEntryPoint`) exposed, we can easily register to it and take control of how long the phase should run

# Service locator

  Simple way to get/set services and optionally adding null factories for services

````c#
// This is optional, but it`s good for services that could scale or change at runtime.
// This also good to have null objects that implements the interface.
public interface ICustomService
{
    public void ServiceAPI();
}

public class ServiceImplementation : ICustomService
{
    public void ServiceAPI()
    {
        // Some logic
    }
}

public class EmptyServiceImplementation : ICustomService
{
    public void ServiceAPI() { } // Do nothing
}

// Loading the service
public class ServiceLoader : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(new ServiceImplementation()); // register as concrete class implementation
        ServiceLocator.Register<ICustomService>(new ServiceImplementation()); // register as interface implementation
        
        // Optionally
        // We can also register a null object implementation on register
        ServiceLocator.Register(new ServiceImplementation(), () => new EmptyServiceImplementation());
        // Or register the factory using their own api
        ServiceLocator.RegisterFactory(() => new EmptyServiceImplementation());
    }
}

// Retrieving the service
public class RandomClassThatUseTheService : MonoBehaviour
{
    private void Start()
    {
        var customServiceInterface = ServiceLocator.Resolve<ICustomService>();
        var customServiceConcrete = ServiceLocator.Resolve<ServiceImplementation>();
        
        customServiceInterface.ServiceAPI();
    }
}
````

# Blocker

  Basically a boolean on steroids, its common to have multiple behaviors blocking/unblocking something, so this blocker 
  allow us to "stack" reasons to block something. 
e.g. Pause/Resume should disable/enable player movement but also an Stop effect should disable/enable movement when casted,
in this case if we unpause during the stop, it will enable the movement even while affected by stop magic. 

```c#
public Blocker movementBlock;

// In some pause controller
public void Pause() => movementBlock.AddBlocker(this, "Pause");
public void Resume() => movementBlock.RemoveBlocker(this);

// In some effect controller
public void ApplyStopEffect() => movementBlock.AddBlocker(this, "Stop effect");
public void RemoveStopEffect() => movementBlock.RemoveBlocker(this);

// In this case the move will happen indepent of the order that blocker are added, we can also retrieve the 
// "reason" that is blocking the behavior to happen
public void Move()
{
    if (movementBlock.IsBlocked)
        return;
    
    // Move
}
```

# NDictionary

​	Uses Odin to serialize a dictionary that persist on nested prefabs scenarios

### WHY:

* Odin's dictionary isn't reliable when working with nested prefabs

### REQUIREMENTS:

* Odin package

### HOW  TO:

​	It uses odin serializer and the 2021 unity's ability to serialize generic fields without declaring a concrete version of it, so you just need to create the field with a serializing attribute

```c#
[SerializeField]
private NDictionary<int, int> myDict;
```

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

​	On some occasions, like when having multiples tasks, to be able to yield for them, you can achieve it calling the `CustomYieldInstruction` `WaitForNTask`

```c#
public void RunRoutine()
    => StartCoroutine(WeCanYieldForTask());

private IEnumerator WeCanYieldForTask()
{
    taskA = new NTask(SomeOperation());
    taskB = new NTask(SomeOtherOperation());

    yield return new WaitForNTask(taskA);
    yield return new WaitForNTask(taskB);
}
```

**NOTE:** It will not start the task for you, so if called with a non-started task it will keep waiting forever



## Know issues:

* Calling a `WaitForEndOfFrame` breaks the task, without any warning, it simple stop there


# Extensions

## Transform
### DestroyChildren()
  Destroy all children from current transform

## Float
### SeparateIntegerAndFractionPart()

# Interfaces

### IMonobehavior
  Contains some props to allow access to runtime components

e.g. Transform
```c#
public Transform Transform => (this as MonoBehaviour)?.transform;
```

# Components
## DisableAtAwake
  Useful component for debug things that should happen outisde play mode only

## ResetRectTransformAtAwake
  Useful to separate HUDs outside game

# Prefabs
## [Debug] HUD Background
  Used for organizations only, used to achieve things like this
  !([https://imgur.com/a/WQr9KXs])

TODO FEAT:

- UI Stack
- UI lib
- Pooler

TODO DOC:

- NTask
    - ??Delay begin??
    - Beggining on next frame??
- Fix MyDebug
- Blocker
- ServiceLocator