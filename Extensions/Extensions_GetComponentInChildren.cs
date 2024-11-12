using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static bool TryGetComponentInChildren<T>(this Component component, Self self, Inactive inactive, out T foundComponent) where T : class
        {
            foundComponent = null;
        
            return self switch
            {
                Self.Include => (foundComponent = component.GetComponentInChildren<T>(inactive == Inactive.Include)) != null,
                _ when component.transform.parent is { } parent && parent != null => (foundComponent =
                    parent.GetComponentInChildren<T>(inactive == Inactive.Include)) != null,
                _ => false
            };
        }

        public static bool TryGetComponentInChildren<T> (this Component component, Self self, out T foundComponent)
            where T : class
            => (foundComponent = component.GetComponentInChildren<T>(self,
                   component.gameObject.activeInHierarchy ? Inactive.Exclude : Inactive.Include))
               != null;

        public static T GetComponentInChildren<T>(this Component component, Self self, Inactive inactive) where T : class
            => self switch
            {
                Self.Include => component.GetComponentInChildren<T>(inactive == Inactive.Include),
                _ when component.transform.parent is { } parent && parent != null => parent.GetComponentInChildren<T>(inactive == Inactive.Include),
                _ => null
            };
 
        public static T GetComponentInChildren<T>(this Component component, Self self) where T : class
            => component.GetComponentInChildren<T>(self, component.gameObject.activeInHierarchy ? Inactive.Exclude : Inactive.Include);
    }
}