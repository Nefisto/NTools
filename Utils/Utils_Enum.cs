using System;
using System.Collections.Generic;
using System.Linq;

public static partial class Utils
{
    /// <summary>
    /// Pretty useful when applying enums to dropdowns 
    /// </summary>
    /// <returns>The enum names in a list</returns>
    public static List<string> GetEnumNames<T>() where T : Enum => Enum.GetNames(typeof(T)).ToList();
}