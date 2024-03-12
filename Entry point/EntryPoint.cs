using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTools
{
    [Serializable]
    public class EntryPoint
    {
        private event Action<IEntryPointContext> NonYieldableListeners;
        private readonly List<Func<IEntryPointContext, IEnumerator>> listeners = new();

        public void Clear()
        {
            NonYieldableListeners = null;
            listeners.Clear();
        }

        public IEnumerator YieldableInvoke (IEntryPointContext ctx = null)
        {
            NonYieldableListeners?.Invoke(ctx);
            
            ctx ??= new EmptyEntryPointContext();

            foreach (var t in listeners.ToList())
                yield return t?.Invoke(ctx);
        }

        /// <summary>
        /// Note that this will invoke JUST the non-yieldable listeners
        /// </summary>
        public void Invoke (IEntryPointContext ctx = null) => NonYieldableListeners?.Invoke(ctx);

        public static EntryPoint operator + (EntryPoint left, Action<IEntryPointContext> right)
        {
            left.NonYieldableListeners += right;
            return left;
        }
        
        public static EntryPoint operator - (EntryPoint left, Action<IEntryPointContext> right)
        {
            left.NonYieldableListeners -= right;
            return left;
        }

        public static EntryPoint operator + (EntryPoint left, Func<IEntryPointContext, IEnumerator> right)
        {
            left.listeners.Add(right);
            return left;
        }

        public static EntryPoint operator - (EntryPoint left, Func<IEntryPointContext, IEnumerator> right)
        {
            left.listeners.Remove(right);
            return left;
        }
        
        private class EmptyEntryPointContext : IEntryPointContext { }
    }
}