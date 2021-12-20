using System;
using UnityEngine;

namespace NTools
{
    public abstract class SingletonMonoBehaviour<T> : LazyBehavior
        where T : Component
    {
        public static T Instance { get; protected set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new Exception("An instance of this singleton already exists.");
            }

            Instance = this as T;
        }
    }
}