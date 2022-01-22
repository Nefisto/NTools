using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static bool CompareWithoutAlpha (this Color color, Color other)
            => Mathf.Abs(color.r - other.r) < 0.001f &&
               Mathf.Abs(color.g - other.g) < 0.001f &&
               Mathf.Abs(color.b - other.b) < 0.001f;
    }
}