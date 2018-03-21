using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelizedObject {
    public static Vector3[] MakeVoxel(Collider collider, float voxelSize)
    {
        Quaternion rotation = collider.transform.rotation;

        collider.transform.rotation = Quaternion.identity;

        Bounds bounds = collider.bounds;


        Vector3Int maxVoxelByAxis = new Vector3Int();
            
        maxVoxelByAxis.x = Mathf.RoundToInt(bounds.size.x / voxelSize);
        maxVoxelByAxis.y = Mathf.RoundToInt(bounds.size.y / voxelSize);
        maxVoxelByAxis.z = Mathf.RoundToInt(bounds.size.z / voxelSize);

        List<Vector3> bufferVoxelList = new List<Vector3>( (int) maxVoxelByAxis.Volume());
        
        for(int i = 0; i < maxVoxelByAxis.x; i++)
        {
            for (int j = 0; j < maxVoxelByAxis.y; j++)
            {
                for (int k = 0; k < maxVoxelByAxis.z; k++)
                {
                    Vector3 point = new Vector3(
                        bounds.min.x + (voxelSize * (0.5f + i)),
                        bounds.min.y + (voxelSize * (0.5f + j)),
                        bounds.min.z + (voxelSize * (0.5f + k)));


                    if (IsVoxelInside(point, collider, ref bounds))
                        bufferVoxelList.Add(point);
                }
            }
        }

        collider.transform.rotation = rotation;

        return bufferVoxelList.ToArray();
    }

    public static bool IsVoxelInside(Vector3 point, Collider collider, ref Bounds colliderBounds)
    {
        float rayLength = colliderBounds.size.magnitude;

        Ray ray = new Ray(point, collider.transform.position - point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider == collider)
                return false;
        }
        return true;
    }

}
