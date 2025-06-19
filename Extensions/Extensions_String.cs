using System.Text.RegularExpressions;

namespace NTools
{
    public static partial class Extensions
    {
        public static string SeparateWordsByCase (this string input)
            => Regex.Replace(input, "([a-z])([A-Z])", "$1 $2")
                .Replace("_", " ")
                .Trim();
    }
}