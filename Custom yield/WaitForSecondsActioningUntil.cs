using System;
using UnityEngine;

namespace NTools
{
    /// <summary>
    /// Will act similar to the WaitForSeconds BUT will pass the amount of time passed until started
    /// </summary>
    public class WaitForSecondsActioningUntil : CustomYieldInstruction
    {
        private float timer = 0f;
        private float counter = 0f;

        private Action<float> action;

        public WaitForSecondsActioningUntil (float seconds, Action<float> action)
        {
            timer = seconds;
            this.action = action;
        }

        public override bool keepWaiting
        {
            get
            {
                counter += Time.deltaTime;

                action?.Invoke(counter);

                return !(counter >= timer);
            }
        }
    }
}