using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTools
{
    public class Pool
    {
        public Component Prefab { get; set; }
        public Transform Root { get; set; }

        public List<Component> Entries { get; set; } = new();
        private List<Component> InUseEntries { get; set; } = new();

        public bool HasAvailableEntries => Entries.Any();

        public void IncreasePool (int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var instance = Object.Instantiate(Prefab, Root, true);
                instance.gameObject.SetActive(false);

                Entries.Add(instance);
            }
        }

        public Component GetEntry()
        {
            var entry = Entries.FirstOrDefault();
            if (entry)
            {
                Entries.Remove(entry);
                InUseEntries.Add(entry);
            }

            return entry;
        }

        public void ReturnEntry (Component entry)
        {
            entry.gameObject.SetActive(false);
            entry.transform.SetParent(Root);

            InUseEntries.Remove(entry);
            Entries.Add(entry);
        }
    }
}