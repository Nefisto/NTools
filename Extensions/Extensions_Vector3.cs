using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        public static Vector3 IgnoreY (this Vector3 vector3) => new(vector3.x, 0f, vector3.z);

        public static Vector2Int RoundToVector2Int (this Vector3 vector3)
            => new(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));

        public static Vector3 RoundToInt (this Vector3 vector3)
            => new(
                Mathf.RoundToInt(vector3.x),
                Mathf.RoundToInt(vector3.y),
                Mathf.RoundToInt(vector3.z));
    }
}