using System;
using System.Collections.Generic;
using System.Linq;

namespace NTools
{
    public static partial class Utils
    {
        /// <summary>
        /// Get enums as strings, pretty useful when applying enums to dropdowns 
        /// </summary>
        /// <returns>The enum names in a list</returns>
        public static List<string> GetEnumNames<T>() where T : Enum => Enum.GetNames(typeof(T)).ToList();
    }
}