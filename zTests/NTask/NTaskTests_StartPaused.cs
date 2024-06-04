using System.Collections;
using NTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public partial class NTaskTests
{
    // TODO: Write about it on README
    [UnityTest]
    public IEnumerator StartPausedIsWorking()
    {
        var task = new NTask(CountingRoutine(), new NTask.Settings() { autoStart = false });
        counter = 0;
        task.Resume();
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        task.Pause();
        
        Assert.IsTrue(counter != 0);
    }
}