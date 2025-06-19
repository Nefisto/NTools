// using System.Collections;
// using NTools;
// using UnityEngine;
// using UnityEngine.Assertions;
// using UnityEngine.TestTools;
//
// public partial class NTaskTests
// {
//     [UnityTest]
//     public IEnumerator OnFinishIsWorkingOnTasksSystemControlledTasks()
//     {
//         var counter = 0;
//         var task = new NTask(YieldForTwoFrames(), new NTask.Settings() { autoStart = false });
//         task.OnFinished += _ => counter++;
//
//         task.Resume();
//         yield return new WaitForSeconds(.1f);
//
//         Assert.AreEqual(counter, 1);
//     }
//
//     [UnityTest]
//     public IEnumerator OnFinishIsWorkingOnTasksManuallyControlled()
//     {
//         var counter = 0;
//         var task = new NTask(YieldForTwoFrames(), new NTask.Settings() { autoStart = false });
//         task.OnFinished += _ => counter++;
//
//         task.MoveNext();
//         task.MoveNext();
//         task.MoveNext();
//         yield return new WaitForSeconds(.1f);
//
//         Assert.AreEqual(counter, 1);
//     }
//     
//     private IEnumerator YieldForTwoFrames()
//     {
//         yield return null;
//         yield return null;
//     }
// }