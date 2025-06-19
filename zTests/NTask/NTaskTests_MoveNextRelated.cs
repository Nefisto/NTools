// using System.Collections;
// using NTools;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
//
// public partial class NTaskTests
// {
//     private static int[] valuesForMoveNextIsWorking = { 1, 3, 5 };
//
//     [UnityTest]
//     public IEnumerator MoveNextIsWorking ([ValueSource(nameof(valuesForMoveNextIsWorking))] int amountOfSteps)
//     {
//         var task = new NTask(CountingRoutine());
//         yield return new WaitForSeconds(Random.Range(.1f, .3f));
//         task.Pause();
//         yield return null;
//
//         var counterAfterPaused = counter;
//
//         var loopCounter = amountOfSteps;
//         while (loopCounter-- > 0)
//             task.MoveNext();
//
//         Assert.AreEqual(counter, counterAfterPaused + amountOfSteps);
//     }
//
//     [UnityTest]
//     public IEnumerator MoveNextSkipBreak()
//     {
//         var localCounter = 0;
//         var task = new NTask(MyMethod());
//         task.Pause();
//         yield return null;
//
//         var currentCounter = localCounter;
//         task.MoveNext();
//         task.MoveNext();
//         
//         Assert.AreEqual(currentCounter + 2, localCounter);
//         
//         IEnumerator MyMethod()
//         {
//             localCounter = 0;
//             while (true)
//             {
//                 yield return RoutineThatBreaks();
//                 yield return RoutineThatBreaks();
//                 localCounter++;
//                 yield return null;
//             }
//         }
//     }
// }