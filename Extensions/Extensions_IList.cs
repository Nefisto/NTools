using System.Collections.Generic;

namespace NTools
{
    public partial class Extensions
    {
        public static bool IsEmpty<T> (this IList<T> source)
            => source.Count == 0;
    }
}