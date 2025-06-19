// using System.Collections;
// using NTools;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
//
// public partial class NTaskTests
// {
//     [UnityTest]
//     public IEnumerator WaitingForSecondsIsWorking()
//     {
//         var hasFinished = false;
//         var task = new NTask(MyRoutine());
//         task.OnFinished += _ => hasFinished = true;
//         yield return new WaitForSeconds(.25f);
//
//         Assert.IsTrue(hasFinished);
//         
//         IEnumerator MyRoutine()
//         {
//             yield return new WaitForSeconds(.2f);
//         }
//     }
// }