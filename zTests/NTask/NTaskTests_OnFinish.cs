using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public partial class NTaskTests
{
    [UnityTest]
    public IEnumerator OnFinishIsWorkingOnTasksSystemControlledTasks()
    {
        var counter = 0;
        var task = new NTask(YieldForTwoFrames(), false);
        task.OnFinished += _ => counter++;

        task.Start();
        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(counter, 1);
    }

    [UnityTest]
    public IEnumerator OnFinishIsWorkingOnTasksManuallyControlled()
    {
        var counter = 0;
        var task = new NTask(YieldForTwoFrames(), false);
        task.OnFinished += _ => counter++;

        task.MoveNext();
        task.MoveNext();
        task.MoveNext();
        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(counter, 1);
    }
    
    private IEnumerator YieldForTwoFrames()
    {
        yield return null;
        yield return null;
    }
}