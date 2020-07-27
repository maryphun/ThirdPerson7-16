using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    [Header("Configurations")]
    public string itemName;
    public GameObject visualMesh;
    [Range(0.0f, 1.0f)]
    public float visualScale = 0.5f;
    [Range(0.0f, 100.0f)]
    public float turnRate = 0.5f;
    [Range(-1.5f, 1.5f)]
    public float YAxis = -1.0f;

    public Vector3 initiateRotate = new Vector3(35f, 90.0f, 0.0f);
    public Collider interactCollider;


    private Transform visual;
    void Start()
    {
        visual = Instantiate(visualMesh, this.transform, false).transform;

        // check if it have a collider. If so, remove it automatically.
        if (visual.GetComponent<MeshCollider>() != null)
        {
            visual.GetComponent<MeshCollider>().enabled = false;
        }

        // make it taller
        visual.position = new Vector3(visual.position.x, visual.position.y + YAxis, visual.position.z);

        // scale it
        visual.localScale = new Vector3(visualScale, visualScale, visualScale);

        // rotate it
        visual.Rotate(new Vector3(initiateRotate.x, initiateRotate.y, initiateRotate.z));
    }
    
    void Update()
    {
        // rotate it
        visual.Rotate(Vector3.down * turnRate * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider target)
    {

    }
}
