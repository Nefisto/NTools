using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTools
{
    [Serializable]
    public class EntryPoint
    {
        private readonly List<Func<IEnumerator>> listeners = new();

        public static EntryPoint operator + (EntryPoint left, Func<IEnumerator> right)
        {
            left.listeners.Add(right);
            return left;
        }
        
        public static EntryPoint operator - (EntryPoint left, Func<IEnumerator> right)
        {
            left.listeners.Add(right);
            return left;
        }
    }
    
    [Serializable]
    public class EntryPoint<T> where T : IEntryPointContext
    {
        protected readonly List<Func<T, IEnumerator>> listeners = new();

        public void Clear() => listeners.Clear();

        public IEnumerator YieldableInvoke (T ctx = default)
        {
            foreach (var t in listeners.ToList())
                yield return t?.Invoke(ctx);
        }
        
        public static EntryPoint<T> operator + (EntryPoint<T> left, Func<T, IEnumerator> right)
        {
            left.listeners.Add(right);
            return left;
        }

        public static EntryPoint<T> operator - (EntryPoint<T> left, Func<T, IEnumerator> right)
        {
            left.listeners.Remove(right);
            return left;
        }
        
        protected class EmptyEntryPointContext : IEntryPointContext { }
    }
}