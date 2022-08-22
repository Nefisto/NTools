using System;
using System.Collections.Generic;

namespace NTools
{
    /// <summary>
    /// Sugar for LINQ distinct
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericComparer<T> : IEqualityComparer<T>
    {
        public event Func<T, T, bool> EqualPredicate;
        public event Func<T, int> GetHashCodeMethod;

        public bool Equals (T x, T y)
            => EqualPredicate(x, y);

        public int GetHashCode (T obj)
            => GetHashCodeMethod(obj);

        public GenericComparer (Func<T, T, bool> equalPredicate, Func<T, int> getHasCodeMethod)
        {
            EqualPredicate += equalPredicate;
            GetHashCodeMethod += getHasCodeMethod;
        }
    }
}