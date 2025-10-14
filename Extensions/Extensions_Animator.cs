using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        /// <summary>
        /// Will yield during the transition to the new animation and then yield until it finishs
        /// </summary>
        public static async UniTask WaitForCurrentAnimationToCompleteAsync (this Animator animator, int layerIndex)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => !animator.IsInTransition(layerIndex));
            await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1f);
        }
    }
}