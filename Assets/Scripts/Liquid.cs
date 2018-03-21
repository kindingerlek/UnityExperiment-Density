using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Liquid : MonoBehaviour {


    [SerializeField]
    private bool visualizeVoxels = false;

    [SerializeField]
    private float voxelSize = 0.5f;

    [SerializeField]
    private float density = 1f;

    [SerializeField]
    private float inLiquidDrag = 1f;

    [SerializeField]
    private float inLiquidAngularDrag = 1f;

    private new Rigidbody rigidbody;
    private new Collider collider;

    private Dictionary<Rigidbody, SubMergedObject> submergedObjs = new Dictionary<Rigidbody, SubMergedObject>();

    private float totalVoxelSubmerge;
    private Vector3 originalSize;


    [System.Serializable]
    private class SubMergedObject
    {
        public Vector3[] voxels;
        public float drag;
        public float angDrag;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        originalSize = transform.parent.localScale;
    }

    private void FixedUpdate()
    {
        float Vfd = totalVoxelSubmerge * Mathf.Pow(voxelSize, 3);
        Vector3 newScale = originalSize;
        newScale.y = originalSize.y + (Vfd / (originalSize.x * originalSize.z));

        transform.parent.localScale = Vector3.Lerp(transform.parent.localScale, newScale,10 * Time.fixedDeltaTime);

        rigidbody.mass = density * originalSize.x * originalSize.y * originalSize.z * 1000f;

        PushAll();

    }
    
    public void OnTriggerEnter(Collider other)
    {
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        if (otherBody == null)
            return;


        if (!submergedObjs.ContainsKey(otherBody))
        {
            SubMergedObject subObject = new SubMergedObject();
            subObject.voxels = VoxelizedObject.MakeVoxel(other, voxelSize);
            subObject.drag = otherBody.drag;
            subObject.angDrag = otherBody.angularDrag;

            submergedObjs.Add(otherBody, subObject);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        if (otherBody == null || !submergedObjs.ContainsKey(otherBody))
            return;

        for(int i = 0; i < submergedObjs.Count; i++)
        {
            submergedObjs[otherBody].voxels = VoxelizedObject.MakeVoxel(other, voxelSize);
        }
    }
        

    //called when something exits the trigger
    public void OnTriggerExit(Collider other)
    {

        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        if (otherBody == null)
            return;

        if (submergedObjs.ContainsKey(otherBody))
        {
            otherBody.drag = submergedObjs[otherBody].drag;
            otherBody.angularDrag = submergedObjs[otherBody].angDrag;


            submergedObjs.Remove(otherBody);
        }
    }

    void PushAll()
    {
        totalVoxelSubmerge = 0;

       foreach(var keyPair in submergedObjs)
        {
            Push(keyPair.Key);
        }
    }

    void Push(Rigidbody other)
    {
        Bounds bounds = collider.bounds;
        int submergedVoxels = 0;

        for (int j = 0; j < submergedObjs[other].voxels.Length; j++)
        {
            Vector3 point = submergedObjs[other].voxels[j];

            if(VoxelizedObject.IsVoxelInside(point, collider,ref bounds))
                submergedVoxels++;
        }

        totalVoxelSubmerge += submergedVoxels;

        float Vfd = submergedVoxels * Mathf.Pow(voxelSize, 3);
        float t = (float) submergedVoxels / (float) submergedObjs[other].voxels.Length;
        
        other.drag        = Mathf.Lerp(submergedObjs[other].drag   ,        inLiquidDrag, t);
        other.angularDrag = Mathf.Lerp(submergedObjs[other].angDrag, inLiquidAngularDrag, t);

        other.AddForce(density * Vfd * -Physics.gravity);
    }

    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying || !visualizeVoxels || submergedObjs == null)
            return;

        Gizmos.color = Color.green;

        List<Rigidbody> rigidbodies = new List<Rigidbody>(submergedObjs.Keys);

        for (int i = 0; i < this.submergedObjs.Count; i++)
        {
            Rigidbody r = rigidbodies[i];
            for (int j = 0; j < submergedObjs[r].voxels.Length; j++)
            {
                Gizmos.DrawCube( submergedObjs[r].voxels[j], Vector3.one * (voxelSize - 0.02f));

            }
        }
    }
}
