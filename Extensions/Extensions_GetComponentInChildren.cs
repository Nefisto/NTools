using System.Collections.Generic;
using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static T NGetComponentInChildren<T> (this Component component, Self self) where T : class
            => component.NGetComponentInChildren<T>(self,
                component.gameObject.activeInHierarchy ? Inactive.Exclude : Inactive.Include);

        public static IEnumerable<T> NGetComponentsInChildren<T> (this Component component, Self self) where T : class
            => component.NGetComponentsInChildren<T>(self,
                component.gameObject.activeInHierarchy ? Inactive.Exclude : Inactive.Include);

        public static T NGetComponentInChildren<T> (this Component component, Self self, Inactive inactive)
            where T : class
        {
            switch (self)
            {
                case Self.Include:
                    return component.GetComponentInChildren<T>(inactive == Inactive.Include);

                case var _ when component.transform.childCount != 0:
                {
                    var includeInactive = inactive == Inactive.Include;
                    foreach (Transform child in component.transform)
                    {
                        if (!child.gameObject.activeInHierarchy && !includeInactive)
                            continue;

                        var foundComponent = child.GetComponentInChildren<T>();

                        if (foundComponent == null)
                            continue;

                        return foundComponent;
                    }


                    return null;
                }

                default:
                    return null;
            }
        }

        private static IEnumerable<T> NGetComponentsInChildren<T> (this Component component, Self self,
            Inactive inactive) where T : class
        {
            switch (self)
            {
                case Self.Include:
                    return component.GetComponentsInChildren<T>(inactive == Inactive.Include);

                case var _ when component.transform.childCount != 0:
                {
                    var components = new List<T>();

                    var includeInactive = inactive == Inactive.Include;
                    foreach (Transform child in component.transform)
                    {
                        if (!child.gameObject.activeInHierarchy && !includeInactive)
                            continue;

                        var foundComponent = child.GetComponentsInChildren<T>(includeInactive);

                        if (foundComponent == null)
                            continue;

                        components.AddRange(foundComponent);
                    }


                    return components;
                }

                default:
                    return null;
            }
        }
    }
}