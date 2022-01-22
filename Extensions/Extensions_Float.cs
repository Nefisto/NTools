using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static bool IsNearlyEnoughTo (this float value, float other, float epsilon = 0.001f)
        {
            return Mathf.Abs(value - other) < epsilon;
        }
        
        /// <summary>
        /// Clamp the value to a max value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="clamp"></param>
        /// <returns></returns>
        public static float NTMaxClamp (this float value, float clamp)
        {
            return value > clamp ? clamp : value;
        }
    }
}