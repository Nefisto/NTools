using UnityEngine;

namespace NTools
{
    public partial class Extensions
    {
        public static bool IsNearlyEnoughTo (this float value, float other, float epsilon = 0.001f)
        {
            return Mathf.Abs(value - other) < epsilon;
        }
    }
}