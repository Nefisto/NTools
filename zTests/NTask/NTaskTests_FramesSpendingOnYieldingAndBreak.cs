using System.Collections;
using NTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public partial class NTaskTests
{
    [UnityTest]
    public IEnumerator RoutineThatJustBreaksShouldFinishOnTheSameFrameThatItsStarted()
    {
        var endFrame = 0;
        var task = new NTask(MyRoutine(), new NTask.Settings { autoStart = false });
        task.OnFinished += _ => endFrame = Time.frameCount;

        yield return null;
        var initialFrame = Time.frameCount;
        task.Resume();

        yield return new WaitForSeconds(.5f);

        Assert.AreEqual(initialFrame, endFrame);

        IEnumerator MyRoutine()
        {
            yield return RoutineThatBreaks();
            yield return RoutineThatBreaks();
            yield return RoutineThatBreaks();
        }
    }

    [UnityTest]
    public IEnumerator RoutineThatYieldOneFrameAndMultipleBreaksShouldTakeExactlyOneFrame()
    {
        var endFrame = 0;
        var task = new NTask(MyRoutine(), new NTask.Settings() { autoStart = false });
        task.OnFinished += _ => endFrame = Time.frameCount;

        yield return null;
        var initialFrame = Time.frameCount;
        task.Resume();

        yield return new WaitForSeconds(.5f);

        Assert.AreEqual(endFrame - initialFrame, 1);

        IEnumerator MyRoutine()
        {
            yield return RoutineThatBreaks();
            yield return RoutineThatTakesOneFrame();
            yield return RoutineThatBreaks();
        }
    }

    [UnityTest]
    public IEnumerator InnerRoutinesAreWaitingTheCorrectAmountOfFrames()
    {
        var endFrame = 0;
        var task = new NTask(MyRoutine(), new NTask.Settings() { autoStart = false });
        task.OnFinished += _ => endFrame = Time.frameCount;

        yield return null;
        var initialFrame = Time.frameCount;
        task.Resume();

        yield return new WaitForSeconds(.5f);

        Assert.AreEqual(4, endFrame - initialFrame);

        IEnumerator MyRoutine()
        {
            yield return RoutineThatBreaksYield1ThanBreaks();
            yield return RoutineThatTakesOneFrame();
            yield return RoutineThatBreaksYield1ThanBreaks();
            yield return null;
        }
    }
}