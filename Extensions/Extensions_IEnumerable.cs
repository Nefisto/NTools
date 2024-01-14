using System;
using System.Collections.Generic;
using System.Linq;

namespace NTools
{
    public static partial class Extensions
    {
        public static T GetRandom<T> (this IEnumerable<T> source)
        {
            return source.GetRandom(1).FirstOrDefault();
        }

        public static IEnumerable<T> GetRandom<T> (this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T> (this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        // <summary>Perform an action on each item.</summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The action to perform.</param>
        public static IEnumerable<T> ForEach<T> (this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
                action(obj);
            return source;
        }

        /// <summary>Perform an action on each item.</summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The action to perform.</param>
        public static IEnumerable<T> ForEach<T> (this IEnumerable<T> source, 
            Action<T, int> action)
        {
            int num = 0;
            foreach (T obj in source)
                action(obj, num++);
            return source;
        }

        // Sugar for distinct using custom generic comparer
        public static IEnumerable<T> Distinct<T> (this IEnumerable<T> source,
            Func<T, T, bool> equalityPredicate,
            Func<T, int> getHashMethod)
        {
            return source.Distinct(new GenericComparer<T>(equalityPredicate, getHashMethod));
        }

        public static IEnumerable<T> Distinct<T> (
            this IEnumerable<T> source,
            Func<T, T, bool> equalityPredicate)
        {
            return source.Distinct(new GenericComparer<T>(equalityPredicate, t => t.GetHashCode()));
        }
    }

}