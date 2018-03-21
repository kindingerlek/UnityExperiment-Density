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

    public static float Volume(this MeshCollider meshCollider)
    {
        return meshCollider.sharedMesh.Volume() * meshCollider.transform.localScale.Volume();
    }

    public static float Volume(this BoxCollider boxCollider)
    {
        return boxCollider.size.Volume() * boxCollider.transform.localScale.Volume();
    }

    public static float Volume(this SphereCollider sphereCollider)
    {
        return (4f * Mathf.Pow(sphereCollider.radius, 3) * Mathf.PI / 3f) * sphereCollider.transform.localScale.Volume();
    }

    public static float Volume(this Mesh mesh)
    {
        float volume = 0f;
        
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            Vector3 a = p1 - p2;
            Vector3 b = p1 - p3;
            Vector3 c = p1;

            float tetraVol = (Vector3.Dot(a, Vector3.Cross(b, c))) / 6f;

            volume += tetraVol;
        }

        return Mathf.Abs(volume);
    }
}
