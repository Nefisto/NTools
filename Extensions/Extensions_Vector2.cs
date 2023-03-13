using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        /// <summary>
        /// Get a random value between X and Y(including)
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static float NTGetRandom (this Vector2 vector2)
            => Random.Range(vector2.x, vector2.y); 
        
        public static Vector2Int ToVector2Int (this Vector2 vector2)
            => new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));

        public static float ToDegreeAngle (this Vector2 vector)
            => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

        public static bool IsNearlyEnoughTo (this Vector2 vector, Vector2 other)
            => vector.x.IsNearlyEnoughTo(other.x) && vector.y.IsNearlyEnoughTo(other.y);
    }
}