using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace NTools
{
    public class EntryPointAsync<T> where T : EventArgs
    {
        private readonly List<Func<object, T, UniTask>> yieldableListeners = new();
        private event Action<object, T> NonYieldableListeners;
        
        public async UniTask InvokeAsync (object sender, T args)
        {
            NonYieldableListeners?.Invoke(sender, args);
    
            foreach (var t in yieldableListeners.ToList())
                await t(sender, args);
        }
    
        public static EntryPointAsync<T> operator + (EntryPointAsync<T> left, Action<object, T> right)
        {
            left.NonYieldableListeners += right;
            return left;
        }
    
        public static EntryPointAsync<T> operator - (EntryPointAsync<T> left, Action<object, T> right)
        {
            left.NonYieldableListeners -= right;
            return left;
        }
    
        public static EntryPointAsync<T> operator + (EntryPointAsync<T> left, Func<object, T, UniTask> right)
        {
            left.yieldableListeners.Add(right);
            return left;
        }
    
        public static EntryPointAsync<T> operator - (EntryPointAsync<T> left, Func<object, T, UniTask> right)
        {
            left.yieldableListeners.Remove(right);
            return left;
        }
    }

    public class EntryPointAsync
    {
        private readonly List<Func<object, EventArgs, UniTask>> yieldableListeners = new();
        private event Action<object, EventArgs> NonYieldableListeners;
    
        public void Clear()
        {
            NonYieldableListeners = null;
            yieldableListeners.Clear();
        }
    
        public async UniTask InvokeAsync (object sender, EventArgs args)
        {
            NonYieldableListeners?.Invoke(sender, args);
    
            foreach (var t in yieldableListeners.ToList())
                await t(sender, args);
        }
    
        public static EntryPointAsync operator + (EntryPointAsync left, Action<object, EventArgs> right)
        {
            left.NonYieldableListeners += right;
            return left;
        }
    
        public static EntryPointAsync operator - (EntryPointAsync left, Action<object, EventArgs> right)
        {
            left.NonYieldableListeners -= right;
            return left;
        }
    
        public static EntryPointAsync operator + (EntryPointAsync left, Func<object, EventArgs, UniTask> right)
        {
            left.yieldableListeners.Add(right);
            return left;
        }
    
        public static EntryPointAsync operator - (EntryPointAsync left, Func<object, EventArgs, UniTask> right)
        {
            left.yieldableListeners.Remove(right);
            return left;
        }
    }
}