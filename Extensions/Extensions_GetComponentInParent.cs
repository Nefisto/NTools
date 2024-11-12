using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static bool TryGetComponentInParents<T>(this Component component, Self self, Inactive inactive, out T foundComponent) where T : class
        {
            foundComponent = null;
        
            return self switch
            {
                Self.Include => (foundComponent = component.GetComponentInParent<T>(inactive == Inactive.Include)) != null,
                _ when component.transform.parent is { } parent && parent != null => (foundComponent =
                    parent.GetComponentInParent<T>(inactive == Inactive.Include)) != null,
                _ => false
            };
        }

        public static bool TryGetComponentInParents<T> (this Component component, Self self, out T foundComponent)
            where T : class
            => (foundComponent = component.GetComponentInParents<T>(self,
                   component.gameObject.activeInHierarchy ? Inactive.Exclude : Inactive.Include))
               != null;

        public static T GetComponentInParents<T>(this Component component, Self self, Inactive inactive) where T : class
            => self switch
            {
                Self.Include => component.GetComponentInParent<T>(inactive == Inactive.Include),
                _ when component.transform.parent is { } parent && parent != null => parent.GetComponentInParent<T>(inactive == Inactive.Include),
                _ => null
            };
 
        public static T GetComponentInParents<T>(this Component component, Self self) where T : class
            => component.GetComponentInParents<T>(self, component.gameObject.activeInHierarchy ? Inactive.Exclude : Inactive.Include);
    
        public enum Self { Exclude, Include }
        public enum Inactive { Exclude, Include }
    }
}