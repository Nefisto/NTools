using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public partial class NTaskTests
{
    private static int[] valuesForMoveNextIsWorking = new[] { 1, 3, 5 };

    [UnityTest]
    public IEnumerator MoveNextIsWorking ([ValueSource(nameof(valuesForMoveNextIsWorking))] int amountOfSteps)
    {
        var task = new NTask(CountingRoutine());
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        task.Pause();
        yield return null;

        var counterAfterPaused = counter;

        var loopCounter = amountOfSteps;
        while (loopCounter-- > 0)
        {
            task.MoveNext();
        }

        Assert.AreEqual(counter, counterAfterPaused + amountOfSteps);
    }
}