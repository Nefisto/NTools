using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public partial class NTaskTests
{
    private NTask taskA, taskB;

    [UnityTest]
    public IEnumerator WeCanYieldForTask()
    {
        var counter = 0;
        taskA = new NTask(WaitingInOneLayer(), false);
        taskB = new NTask(WaitingInMultipleLayer(), false);

        yield return taskA.GetEnumerator();
        counter++;
        yield return taskB.GetEnumerator();
        counter++;
        
        Assert.AreEqual(counter, 2);
    }

    private IEnumerator WaitingInOneLayer()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator WaitingInMultipleLayer()
    {
        yield return WaitForTwoFrames();
        yield return new WaitForEndOfFrame();
        yield return AnotherLayer();
    }

    private IEnumerator AnotherLayer()
    {
        yield return WaitForTwoFrames();
    }
    
    private IEnumerator WaitForTwoFrames()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }
}