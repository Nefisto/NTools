// This idea came from Ryan Hipple talk: https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=184s 

using System.Collections.Generic;
using UnityEngine;

namespace NTools
{
    [CreateAssetMenu(fileName = "RuntimeSet", menuName = "NTools/RuntimeSet")]
    public class RuntimeSet : ScriptableObject
    {
        public List<RuntimeItem> items;

        public int Count => items.Count;

        public void Add (RuntimeItem thing)
        {
            if (!items.Contains(thing))
                items.Add(thing);
        }

        public void Remove (RuntimeItem thing)
        {
            if (items.Contains(thing))
                items.Remove(thing);
        }
    }
}