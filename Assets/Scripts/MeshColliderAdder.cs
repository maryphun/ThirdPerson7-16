using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderAdder : MonoBehaviour
{
    void Start()
    {
        // Iterate through all child objects of our Geometry object
        foreach (Transform childObject in transform)
        {
            // Determine if they need a mesh collider
            AddMeshCollider(childObject);
        }
        Debug.Log("added mesh");
    }

    private void AddMeshCollider(Transform target)
    {
        // First we get the Mesh attached to the child object
        Mesh mesh = target.gameObject.GetComponent<MeshFilter>().mesh;

        // Only add mesh to the child if it have no mesh
        if (target.gameObject.GetComponent<MeshCollider>() == null)
        {
            // If we've found a mesh we can use it to add a collider
            if (mesh != null)
            {
                // Add a new MeshCollider to the child object
                MeshCollider meshCollider = target.gameObject.AddComponent<MeshCollider>();

                // Finaly we set the Mesh in the MeshCollider
                meshCollider.sharedMesh = mesh;
            }
        }

        // After that check if the child have their own child attached
        if (target.childCount > 0)
        {
            foreach (Transform childchildObject in target)
            {
                // Determine if they need a mesh collider
                AddMeshCollider(childchildObject);
            }
        }
    }
}
