using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTools
{
    [Serializable]
    public class EntryPoint
    {
        private readonly List<Func<IEntryPointContext, IEnumerator>> actions = new();

        public void Add (Func<IEntryPointContext, IEnumerator> callback) => actions.Add(callback);
        public void Remove (Func<IEntryPointContext, IEnumerator> callback) => actions.Remove(callback);

        public IEnumerator Run (IEntryPointContext ctx = null)
        {
            ctx ??= new EmptyEntryPointContext();

            foreach (var t in actions.ToList())
                yield return t?.Invoke(ctx);
        }

        public void Clear() => actions.Clear();

        private class EmptyEntryPointContext : IEntryPointContext { }
    }
}