using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float Area(this Vector2 v)
    {
        return v.x * v.y;
    }

    public static float Area(this Vector2Int v)
    {
        return v.x * v.y;
    }

    public static float Volume(this Vector3 v)
    {
        return v.x * v.y * v.z;
    }

    public static float Volume(this Vector3Int v)
    {
        return v.x * v.y * v.z;
    }
}
