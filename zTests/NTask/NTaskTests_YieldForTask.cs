using System.Collections;
using NTools;
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
        taskA = new NTask(WaitingInOneLayer());
        taskB = new NTask(WaitingInMultipleLayer());

        yield return new WaitForNTask(taskA);
        counter++;
        yield return new WaitForNTask(taskB);
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