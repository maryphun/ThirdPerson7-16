using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    [Header("Object Reference")]
    public GameObject hpbar;
    public Rigidbody rigidbody;
    public Collider collider;

    [Header("General Properties")]
    public string enemyName;

    [Header("Float Properties")]
    public float speed = 0.001f;
    public float frontSpeedMultiplier = 1.25f;
    public float animationTransmitionRate = 5.0f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothVelocity;
    public float hitpoint = 1.0f;
    [Range(0.0f, 0.5f)]
    public float hpBarOffset = 0.0f;

    //private
    private GameObject hpbarHandle;
    private Camera cam;
    private float hpcurrent;
    private float horizontal;
    private float vertical;
    private float moveSpeed;    //speed after multiplier this frame

    void Start()
    {
        hpcurrent = hitpoint;
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = Vector3.zero;
        if (hpbarHandle != null)
        {
            //update hp bar position
            Vector3 barPos = transform.position;
            barPos.y += (collider.bounds.size.y / 2f) + hpBarOffset;
            hpbarHandle.transform.position = cam.WorldToScreenPoint(barPos);
        }
    }

    public void TakeDamage(float damage)
    {
        if (hpbarHandle == null)
        {
            //instiate prefab
            hpbarHandle = Instantiate(hpbar);
            hpbarHandle.transform.SetParent(cam.transform.GetChild(0), false);
            hpbarHandle.SetActive(true);
            //set position
            Vector3 barPos = transform.position;
            barPos.y += (collider.bounds.size.y / 2f) + hpBarOffset;
            hpbarHandle.transform.position = cam.WorldToScreenPoint(barPos);
            hpbarHandle.GetComponent<CanvasProperties>().SetTextMesh(0, enemyName);
            hpbarHandle.GetComponent<CanvasProperties>().SetAlpha(1.0f, 0.09f, 3.5f);
        }
        else
        {
            hpbarHandle.GetComponent<CanvasProperties>().SetAlpha(1.0f, 0.09f, 3.5f);
        }
    }

    public float GetCurrentHitPoint()
    {
        return hpcurrent;
    }
}
