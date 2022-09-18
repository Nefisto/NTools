// This idea came from Ryan Hipple talk: https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=184s 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTools
{
    [CreateAssetMenu(fileName = "RuntimeSet", menuName = "NTools/RuntimeSet")]
    public class RuntimeSet : ScriptableObject, IList<RuntimeItem>
    {
        [SerializeField]
        private List<RuntimeItem> items;
        
        public IEnumerator<RuntimeItem> GetEnumerator()
            => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)items).GetEnumerator();

        public void Add (RuntimeItem item)
            => items.Add(item);

        public void Clear()
            => items.Clear();

        public bool Contains (RuntimeItem item)
            => items.Contains(item);

        public void CopyTo (RuntimeItem[] array, int arrayIndex)
            => items.CopyTo(array, arrayIndex);

        public bool Remove (RuntimeItem item)
            => items.Remove(item);

        public int Count => items.Count;

        public bool IsReadOnly => ((ICollection<RuntimeItem>)items).IsReadOnly;

        public int IndexOf (RuntimeItem item)
            => items.IndexOf(item);

        public void Insert (int index, RuntimeItem item)
            => items.Insert(index, item);

        public void RemoveAt (int index)
            => items.RemoveAt(index);

        public RuntimeItem this [int index]
        {
            get => items[index];
            set => items[index] = value;
        }
    }
}