using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        /// <summary>
        /// Will yield during the transition to the new animation and then yield until it finishs
        /// </summary>
        /// <param name="shouldDebug">Some messages to show where the animation current are</param>
        public static async UniTask WaitForCurrentAnimationToCompleteAsync (this Animator animator,
            bool shouldDebug = false)
        {
            await UniTask.Yield();

            while (animator.IsInTransition(0))
                await UniTask.Yield();

            var animationName = GetCurrentAnimationName();
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (shouldDebug)
                Debug.Log($"Waiting for animation {animationName}");

            await UniTask.Yield();

            while (stateInfo.IsName(animationName))
            {
                var normalizedTime = stateInfo.normalizedTime;

                if (shouldDebug)
                    Debug.Log($"Animation {animationName} normalizedTime: {normalizedTime}");

                if (normalizedTime >= 1f)
                {
                    if (shouldDebug)
                        Debug.Log($"Finished waiting for animation {animationName}");

                    return;
                }

                await UniTask.Yield();
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            string GetCurrentAnimationName()
            {
                var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                return clipInfo.Length > 0 ? clipInfo[0].clip.name : "No animation";
            }
        }
    }
}