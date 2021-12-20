using UnityEngine;

namespace NTools
{
    public partial class Extensions
    {
        public static Vector2Int RoundToVector2Int (this Vector3 vector3)
            => new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));

        public static Vector3 RoundToInt (this Vector3 vector3)
            => new Vector3(
                Mathf.RoundToInt(vector3.x),
                Mathf.RoundToInt(vector3.y),
                Mathf.RoundToInt(vector3.z));
    }
}