using UnityEngine;

public static partial class Extensions
{
    public static Vector3 NRandomizePosition (this BoxCollider collider)
    {
        var boundary = collider.bounds;

        var randomizedX = Random.Range(boundary.min.x, boundary.max.x);
        var randomizedY = Random.Range(boundary.min.y, boundary.max.y);
        var randomizedZ = Random.Range(boundary.min.z, boundary.max.z);

        return new Vector3(randomizedX, randomizedY, randomizedZ);
    }
}