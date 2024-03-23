using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTools
{
    [Serializable]
    public class NDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        [HideInInspector]
        private List<TKey> keyData = new();

        [SerializeField]
        [HideInInspector]
        private List<TValue> valueData = new();

        public NDictionary() { }

        public NDictionary(IEnumerable<TKey> keys, IEnumerable<TValue> elements)
        {
            var keysList = keys.ToArray();
            var elementsList = elements.ToArray();
            for (var i = 0; i < keysList.Length; i++)
            {
                if (ContainsKey(keysList[i]))
                    continue;
                
                Add(keysList[i], elementsList[i]);
            }
        }
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            for (var i = 0; i < keyData.Count && i < valueData.Count; i++)
                this[keyData[i]] = valueData[i];
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            keyData.Clear();
            valueData.Clear();
            foreach (var (key, value) in this)
            {
                keyData.Add(key);
                valueData.Add(value);
            }
        }
    }
}