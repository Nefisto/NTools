using UnityEngine;

namespace NTools
{
    public class WaitForNTask : CustomYieldInstruction
    {
        private NTask task;
        public WaitForNTask (NTask task) => this.task = task;

        public override bool keepWaiting => task.IsRunning;
    }
}