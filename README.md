# Summary <a name="TOC">

- [Entry point](#EntryPoint)
- [Service locator](#ServiceLocator)
- [Blocker](#Blocker)
- NDictionary
- NTask
- [Custom yield](#CustomYield)
- [Singleton ScriptableObject](#SingletonSO)
- [Fade screen service](#FadeScreen)
- [UI stack service](#UIStack)
- [Utils](#Utils)
- Extensions
- Interfaces
- Components
- Prefabs
- [Editor tools](#EditorTools)

# [ENTRY POINTS ASYNC](#TOC) <a name="EntryPoint">

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

# [Service locator](#TOC) <a name="ServiceLocator">

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

# [Blocker](#TOC) <a name="Blocker">

  Basically a boolean on steroids, its common to have multiple behaviors blocking/unblocking something, so this blocker 
  allow us to "stack" reasons to block something. 
e.g. Pause/Resume should disable/enable player movement but also an Stop effect should disable/enable movement when casted,
in this case if we unpause during the stop, it will enable the movement even while affected by stop magic. 

​	Each *owner* (the object passed as first argument) can stack multiple *reasons*, so the same owner can block for more than one motive at the same time. Adding the same `(owner, reason)` twice is treated as a logic error and logs an error, since it should never happen.

```c#
public Blocker movementBlock;

// In some pause controller
public void Pause() => movementBlock.AddBlocker(this, "Pause");
public void Resume() => movementBlock.RemoveBlocker(this, "Pause");

// In some effect controller, the same owner can stack more than one reason
public void ApplyStopEffect() => movementBlock.AddBlocker(this, "Stop effect");
public void ApplyRootEffect() => movementBlock.AddBlocker(this, "Root effect");
public void RemoveStopEffect() => movementBlock.RemoveBlocker(this, "Stop effect"); // removes only this reason
public void ClearAllEffects() => movementBlock.RemoveBlocker(this);                 // removes every reason of this owner

// In this case the move will happen indepent of the order that blocker are added, we can also retrieve the 
// "reasons" that are blocking the behavior to happen
public void Move()
{
    if (movementBlock.IsBlocked)
    {
        // GetBlockers() returns every ReasonData blocking, GetBlockers(owner) only the ones of that owner
        foreach (var reason in movementBlock.GetBlockers())
            Debug.Log(reason); // ReasonData converts implicitly to string, printing its Reason
        
        return;
    }
    
    // Move
}
```

* `RemoveBlocker(owner, reason)` removes a single reason, `RemoveBlocker(owner)` clears every reason of that owner at once (handy on scene changes)
* `ReasonData` currently only holds a `Reason` string (and converts implicitly to it), but exists as a class so it can carry more data later

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


# [Custom yield](#TOC) <a name="CustomYield">

​	`WaitForSecondsActioningUntil` works like `WaitForSeconds` but invokes a callback every frame with the elapsed time until it completes, useful to drive progress feedback while waiting

```c#
// Waits 3 seconds while reporting how much time has passed each frame
yield return new WaitForSecondsActioningUntil(3f, elapsed => progressBar.fillAmount = elapsed / 3f);
```

# [Singleton ScriptableObject](#TOC) <a name="SingletonSO">

​	Base class to turn a `ScriptableObject` into a singleton loaded from `Resources`. Set `InstancePath` to the folder inside a `Resources` folder and access it through `Instance`. It logs an error if none or multiple instances are found

```c#
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    public int startingLives;
}

// Anywhere
var lives = GameConfig.Instance.startingLives;
```

# [Fade screen service](#TOC) <a name="FadeScreen">

  Simple async service to fade an UI `Image` in and out

### REQUIREMENTS:

* UniTask package

```c#
var fade = new FadeScreenService(new FadeScreenService.Settings
{
    FadeImage = myImage,
    FadeDuration = 0.5f,
    Color = Color.black
});

await fade.FadeInAsync();  // goes to full alpha and starts blocking raycasts
await fade.FadeOutAsync(); // goes to zero alpha and releases raycasts
```

* You can pass a per-call `Settings` to `FadeInAsync`/`FadeOutAsync` to override the default one

# [UI stack service](#TOC) <a name="UIStack">

  Async stack of screens. `Push` opens a screen on top of the others; the ones underneath stay visible but have their input blocked until they become the top again. `Pop` closes the top screen and re-enables the one revealed beneath it.

### REQUIREMENTS:

* UniTask package

​	Screens implement `IUIScreen`, which requires `IMonobehavior` (so it must live on a `MonoBehaviour`) and exposes the async open/close:

```c#
public class MyScreen : MonoBehaviour, IUIScreen
{
    public async UniTask OpenAsync()  { /* fade / slide in  */ }
    public async UniTask CloseAsync() { /* fade / slide out */ }
}
```

​	Then drive them through the service:

```c#
var ui = new UIStackService();

var settings = await ui.PushAsync(settingsScreen); // opens on top, returns the pushed screen
await ui.PushAsync(confirmPopup);                  // stacks another and blocks input on settingsScreen
await ui.PopAsync();                               // closes confirmPopup, re-enables settingsScreen
await ui.PopAllAsync();                            // closes everything from the top down
```

* Input blocking is automatic: while a screen is covered the service flips `blocksRaycasts` on a `CanvasGroup` at the screen's root (adding one if missing), so there's no need to position a full-screen [Blocker](#Blocker) object on the canvas. Because it reaches the screen through its own `Transform`, `IUIScreen` extends `IMonobehavior`
* `Current` is the top screen (or `null`) and `Count` is the stack size
* `NullUIStackService` is a do-nothing implementation for the [service locator](#ServiceLocator) null-object pattern
* There's a runtime-built sample under `_Samples/UI Stack` — open the scene and press Play to see the stacking and input blocking in action

# [Utils](#TOC) <a name="Utils">

## GetEnumNames\<T>()
  Returns the names of an enum as a `List<string>`, useful for populating dropdowns

```c#
var names = Utils.GetEnumNames<MyEnum>();
```

# Extensions

## Transform
### DestroyChildren()
  Destroy all children GameObjects from current transform
### DisableChildren()
  Set all children GameObjects inactive

## RectTransform
### GetSizeBetweenAnchor()
  Returns the size between the anchors

## Float
### SeparateIntegerAndFractionPart()
  Splits the value into its `(int integer, float fraction)` parts
### IsNearlyEnoughTo(float other, float epsilon = 0.001f)
  True if both values differ by less than `epsilon`

## String
### SeparateWordsByCase()
  Inserts spaces at camelCase boundaries and replaces underscores with spaces (e.g. `"myValue_here"` -> `"my Value here"`)

## Color
### SetAlpha(float targetAlpha)
  Returns the color with its alpha replaced (returns a `Color32`)
### CompareWithoutAlpha(Color other)
  True if the RGB channels match (alpha ignored)

## Vector2
### GetRandom()
  Random value between `x` and `y` (treats the vector as a min/max range)
### ToVector2Int()
  Rounds each component to the nearest int
### ToDegreeAngle()
  Angle of the vector in degrees
### IsNearlyEnoughTo(Vector2 other)
  True if both components are nearly equal

## Vector2Int
### ToDegreeAngle()
  Angle of the vector in degrees
### ToVector3()
  Converts to `Vector3` with `z = 0`
### ToVector2()
  Converts to `Vector2`

## Vector3
### IgnoreY()
  Returns the vector with `y` zeroed
### RoundToInt()
  Rounds all components to the nearest int
### RoundToVector2Int()
  Rounds `x` and `y` to ints (drops z)

## Texture2D
### ToSprite()
  Creates a full-size sprite from the texture
### TrimTransparentPixels()
  Returns a new texture cropped to the smallest rect containing all non-transparent pixels
### SaveTextureToFile(string filePath, bool overwrite = false, bool usePngFormat = true)
  Encodes (PNG or JPG) and writes to a file — **requires UniTask**
### SaveToProjectFolder(string relativePath, bool overwrite = false, bool usePngFormat = true)
  Saves the texture under an `Assets/`-relative path, creating folders and refreshing the AssetDatabase — **requires UniTask**

## Animator
### WaitForCurrentAnimationToCompleteAsync(int layerIndex)
  Awaits until the current state on the layer finished playing — **requires UniTask**

## BoxCollider
### RandomizePosition()
  Returns a random world-space point inside the collider's bounds

## IList
### IsEmpty()
  True when `Count` is 0

## IEnumerable
### GetRandom()
  Returns one random element (or default if empty)
### GetRandom(int count)
  Returns `count` random elements
### Shuffle()
  Returns the sequence in randomized order
### ForEach(Action<T> action) / ForEach(Action<T, int> action)
  Runs the action on each item (optionally with its index) and returns the source
### Distinct(Func<T, T, bool> equalityPredicate, Func<T, int> getHashMethod = null)
  Distinct using a custom equality predicate (and optional hash function)
### ToNDictionary(keySelector, elementSelector)
  Builds an `NDictionary` from the sequence

## GenericComparer\<T>
  `IEqualityComparer<T>` that wraps a custom equality predicate and hash function, handy for the LINQ overloads that ask for one

## GetComponent helpers
  Component lookups that let you control whether to include the object itself (`Self.Include/Exclude`) and inactive objects (`Inactive.Include/Exclude`)

### NGetComponentInChildren\<T>(Self self, Inactive inactive) / NGetComponentsInChildren\<T>(Self self)
  Get one/all components of type `T` in children (inactive inclusion auto-picked from active state when the `Inactive` argument is omitted)
### GetComponentInParents\<T>(Self self, Inactive inactive) / TryGetComponentInParents\<T>(Self self, out T found)
  Get (or try-get) a component of type `T` walking up the parents

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

## DoNotDestroyOnLoad
  Unparents the game object and keeps it alive across scene loads

## Information
  Lets you write a note on the inspector to document a game object

# Prefabs
## [Debug] HUD Background
  Used for organizations only, used to achieve things like this
  !([https://imgur.com/a/WQr9KXs])

# [Editor tools](#TOC) <a name="EditorTools">

## Version increment
  Menu items under `Tools/NTools/Version` to bump the project's bundle version (Patch/Minor/Major)

## Font changer
  Window under `Tools/NTools/Font Changer` to apply a `TMP_FontAsset` to every TextMeshPro text in the open scene