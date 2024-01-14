using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static bool IsNearlyEnoughTo (this float value, float other, float epsilon = 0.001f)
            => Mathf.Abs(value - other) < epsilon;
    }
}