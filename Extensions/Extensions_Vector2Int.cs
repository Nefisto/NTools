using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static float ToDegreeAngle (this Vector2Int vector)
            => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

        public static Vector3 ToVector3 (this Vector2Int vector)
            => new Vector3(vector.x, vector.y, 0f);

        public static Vector2 ToVector2 (this Vector2Int vector)
            => new Vector2(vector.x, vector.y);
    }
}