using System;

namespace NTools
{
    public abstract class InvokableActionBase { }

    public class InvokableAction : InvokableActionBase
    {
        public InvokableAction (Action action)
            => this.action = action;

        private event Action action;

        public void Invoke()
            => action?.Invoke();

        public bool IsSameAction (InvokableActionBase other)
            => other is InvokableAction invokableAction
               && invokableAction.action == action;
    }

    public class InvokableAction<T1> : InvokableActionBase
    {
        public InvokableAction (Action<T1> action)
            => this.action = action;

        private event Action<T1> action;

        public void Invoke (T1 arg)
            => action?.Invoke(arg);
        
        public bool IsSameAction (InvokableActionBase other)
            => other is InvokableAction<T1> invokableAction
               && invokableAction.action == action;
    }

    public class InvokableAction<T1, T2> : InvokableActionBase
    {
        public InvokableAction (Action<T1, T2> action)
            => this.action = action;

        private event Action<T1, T2> action;

        public void Invoke (T1 arg1, T2 arg2)
            => action?.Invoke(arg1, arg2);
        
        public bool IsSameAction (InvokableActionBase other)
            => other is InvokableAction<T1, T2> invokableAction
               && invokableAction.action == action;
    }
}