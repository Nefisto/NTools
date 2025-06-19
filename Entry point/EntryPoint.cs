using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTools
{
    [Obsolete("Use EntryPointAsync instead")]
    public class EntryPoint
    {
        private readonly List<Func<object, EventArgs, IEnumerator>> yieldableListeners = new();
        private event Action<object, EventArgs> NonYieldableListeners;

        public void Clear()
        {
            NonYieldableListeners = null;
            yieldableListeners.Clear();
        }

        public IEnumerator YieldableInvoke (object sender, EventArgs args)
        {
            NonYieldableListeners?.Invoke(sender, args);

            foreach (var t in yieldableListeners.ToList())
                yield return t?.Invoke(sender, args);
        }

        public static EntryPoint operator + (EntryPoint left, Action<object, EventArgs> right)
        {
            left.NonYieldableListeners += right;
            return left;
        }

        public static EntryPoint operator - (EntryPoint left, Action<object, EventArgs> right)
        {
            left.NonYieldableListeners -= right;
            return left;
        }

        public static EntryPoint operator + (EntryPoint left, Func<object, EventArgs, IEnumerator> right)
        {
            left.yieldableListeners.Add(right);
            return left;
        }

        public static EntryPoint operator - (EntryPoint left, Func<object, EventArgs, IEnumerator> right)
        {
            left.yieldableListeners.Remove(right);
            return left;
        }
    }
}