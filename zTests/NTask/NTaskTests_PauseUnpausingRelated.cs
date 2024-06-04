using System.Collections;
using NTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public partial class NTaskTests
{
    [UnityTest]
    public IEnumerator PausingIsWorking()
    {
        var task = new NTask(CountingRoutine());
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        task.Pause();
        yield return null;
        var counterAfterPaused = counter;
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        
        Assert.AreEqual(counterAfterPaused, counter);
    }
    
    [UnityTest]
    public IEnumerator ResumeIsWorking()
    {
        var task = new NTask(CountingRoutine());
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        task.Pause();
        yield return null;
        var counterAfterPaused = counter;
        task.Resume();
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        
        Assert.AreNotEqual(counterAfterPaused, counter);
    }
}