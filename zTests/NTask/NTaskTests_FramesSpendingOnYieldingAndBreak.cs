using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public partial class NTaskTests
{
    [UnityTest]
    public IEnumerator RoutineThatJustBreaksShouldFinishOnTheSameFrameThatItsStarted()
    {
        var endFrame = 0;
        var task = new NTask(MyRoutine(), false);
        task.OnFinished += _ => endFrame = Time.frameCount;

        yield return null;
        var initialFrame = Time.frameCount;
        task.Start();
        
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
        var task = new NTask(MyRoutine(), false);
        task.OnFinished += _ => endFrame = Time.frameCount;

        yield return null;
        var initialFrame = Time.frameCount;
        task.Start();
        
        yield return new WaitForSeconds(.5f);

        Assert.AreEqual(endFrame - initialFrame, 1);
        
        IEnumerator MyRoutine()
        {
            yield return RoutineThatBreaks();
            yield return RoutineThatTakesOneFrame();
            yield return RoutineThatBreaks();
        }
    }
}