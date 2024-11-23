using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTools
{
    [Serializable]
    public class EntryPoint
    {
        private readonly List<Func<IEnumerator>> yieldableListener = new();

        // Used to be able to apply lambda as listeners
        private Action nonYieldableListener;

        public void Clear()
        {
            nonYieldableListener = null;
            yieldableListener.Clear();
        }

        public IEnumerator YieldableInvoke()
        {
            nonYieldableListener?.Invoke();

            foreach (var t in yieldableListener.ToList())
                yield return t?.Invoke();
        }

        public static EntryPoint operator + (EntryPoint left, Action right)
        {
            left.nonYieldableListener += right;
            return left;
        }

        public static EntryPoint operator - (EntryPoint left, Action right)
        {
            left.nonYieldableListener -= right;
            return left;
        }

        public static EntryPoint operator + (EntryPoint left, Func<IEnumerator> right)
        {
            left.yieldableListener.Add(right);
            return left;
        }

        public static EntryPoint operator - (EntryPoint left, Func<IEnumerator> right)
        {
            left.yieldableListener.Remove(right);
            return left;
        }
    }

    [Serializable]
    public class EntryPoint<T> where T : class
    {
        private readonly List<Func<T, IEnumerator>> yieldableListeners = new();
        private event Action<T> NonYieldableListeners;

        public void Clear()
        {
            NonYieldableListeners = null;
            yieldableListeners.Clear();
        }

        public IEnumerator YieldableInvoke (T ctx = null)
        {
            NonYieldableListeners?.Invoke(ctx);

            foreach (var t in yieldableListeners.ToList())
                yield return t?.Invoke(ctx);
        }

        public static EntryPoint<T> operator + (EntryPoint<T> left, Action<T> right)
        {
            left.NonYieldableListeners += right;
            return left;
        }

        public static EntryPoint<T> operator - (EntryPoint<T> left, Action<T> right)
        {
            left.NonYieldableListeners -= right;

            return left;
        }

        public static EntryPoint<T> operator + (EntryPoint<T> left, Func<T, IEnumerator> right)
        {
            left.yieldableListeners.Add(right);
            return left;
        }

        public static EntryPoint<T> operator - (EntryPoint<T> left, Func<T, IEnumerator> right)
        {
            left.yieldableListeners.Remove(right);
            return left;
        }
    }

    [Serializable]
    public class EntryPoint<T, K>
        where T : class
        where K : EventArgs
    {
        private readonly List<Func<T, IEnumerator>> yieldableListeners = new();
        private event Action<T> NonYieldableListeners;

        public void Clear()
        {
            NonYieldableListeners = null;
            yieldableListeners.Clear();
        }

        public IEnumerator YieldableInvoke (T ctx = null)
        {
            NonYieldableListeners?.Invoke(ctx);

            foreach (var t in yieldableListeners.ToList())
                yield return t?.Invoke(ctx);
        }

        public static EntryPoint<T, K> operator + (EntryPoint<T, K> left, Action<T> right)
        {
            left.NonYieldableListeners += right;
            return left;
        }

        public static EntryPoint<T, K> operator - (EntryPoint<T, K> left, Action<T> right)
        {
            left.NonYieldableListeners -= right;

            return left;
        }

        public static EntryPoint<T, K> operator + (EntryPoint<T, K> left, Func<T, IEnumerator> right)
        {
            left.yieldableListeners.Add(right);
            return left;
        }

        public static EntryPoint<T, K> operator - (EntryPoint<T, K> left, Func<T, IEnumerator> right)
        {
            left.yieldableListeners.Remove(right);
            return left;
        }
    }
}