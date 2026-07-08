using System;
using System.Collections.Generic;
using UnityEngine;

namespace NTools
{
    public class PoolService : IDisposable
    {
        private readonly Settings settings;

        private Dictionary<Type, Pool> pools = new();

        public PoolService (Settings settings)
        {
            this.settings = settings;
        }

        public void Dispose() { }

        public void CreatePool<T> (T prefab, Settings settings = null) where T : Component
        {
            settings ??= this.settings;

            if (!pools.TryGetValue(prefab.GetType(), out var pool))
            {
                pool = CreatePoolSettings(prefab, settings);
                pools.Add(prefab.GetType(), pool);
            }

            pool.IncreasePool(settings.StepAmount);
        }

        public T Get<T>() where T : Component
        {
            if (!pools.TryGetValue(typeof(T), out var pool))
            {
                Debug.LogError($"Pool of type {typeof(T)} not found, you need to create the pool first before use it");
                return null;
            }

            if (!pool.HasAvailableEntries)
                pool.IncreasePool(settings.StepAmount);

            var entry = pool.GetEntry();
            return (T)entry;
        }

        public void Return (Component entry)
        {
            if (!pools.TryGetValue(entry.GetType(), out var pool))
            {
                Debug.LogError(
                    $"Pool of type {entry.GetType()} not found, you need to create the pool first before use it");
                return;
            }

            pool.ReturnEntry(entry);
        }

        private static Pool CreatePoolSettings<T> (T prefab, Settings settings) where T : Component
        {
            var root = new GameObject($"Pool of: {prefab.GetType()}");
            root.transform.SetParent(settings.PoolsRoot);

            return new Pool()
            {
                Root = root.transform,
                Prefab = prefab,
                Entries = new List<Component>()
            };
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public int StepAmount { get; private set; } = 30;

            [field: SerializeField]
            public Transform PoolsRoot { get; private set; }
        }
    }
}