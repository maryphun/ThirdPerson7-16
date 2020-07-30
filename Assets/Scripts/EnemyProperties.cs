using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyProperties : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField]
    private GameObject hpbar;
    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private Collider triggerCollider;
    [SerializeField]
    private Transform hpBarRoot;
    [SerializeField]
    private EnemyAnimation animationController;
    [SerializeField]
    private GameObject model;

    [Header("General Properties")]
    public string enemyName;
    public Gradient hpBarColor;
    public Color hpBarFollowColor;

    [Header("Float Properties")]
    public float speed = 0.001f;
    public float frontSpeedMultiplier = 1.25f;
    public float animationTransmitionRate = 5.0f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothVelocity;
    public float hitpoint = 1.0f;
    [Range(-1.0f, 1.0f)]
    public float hpBarOffset = 0.0f;

    //delegates
    public delegate void EnemySpecificFuntion();
    public EnemySpecificFuntion func;

    //private
    [HideInInspector]
    private float hpBarDisplaySpeed = 0.09f;
    private float hpBarDisplayTime = 3.5f;
    private float hpBarLengthFormula = 4.26603870577f; // this is the best balance I found and not thinking to modifiy it anyway.
    private float hpBarFillDamping;
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
        //rigidbody.velocity = Vector3.zero;
        if (hpbarHandle != null)
        {
            var canvas = hpbarHandle.GetComponent<CanvasProperties>();
            
            //update hp bar position
            Vector3 barPos = hpBarRoot.position;
            barPos.y += hpBarOffset;

            Vector3 viewPos = cam.WorldToViewportPoint(barPos);
        
            hpbarHandle.transform.position = cam.WorldToScreenPoint(barPos);
            // make sure it wont go out of camera screen border if this enemy is in screen
            if (IsObjectInSight(transform, cam) && canvas.GetAlpha() > 0f)
            {
                if (cam.WorldToViewportPoint(barPos).y >= 1f)
                {
                    float hpbarHeight = (hpbarHandle.transform.GetChild(0).GetComponent<RectTransform>().rect.height);
                    hpbarHandle.transform.position = new Vector2(hpbarHandle.transform.position.x, Screen.height - hpbarHeight);
                }
            }
            canvas.SetProgressorColor(0, hpBarColor, hpcurrent / hitpoint);
            canvas.SetProgressor(0, hpcurrent / hitpoint);
            canvas.SetProgressor(1, Mathf.MoveTowards(canvas.GetProgressor(1), hpcurrent / hitpoint, 0.25f * Time.deltaTime));
        }
    }

    public void TakeDamage(float damage)
    {
        hpcurrent = Mathf.Clamp(hpcurrent - damage, 0.0f, hitpoint);

        if (hpcurrent == 0)
        {
            // remove its collider so the enemy won't got hit again
            triggerCollider.enabled = false;

            // tell the animator controller this unit is death
            animationController.Death();

            // Add different collider
            rigidbody.isKinematic = true;

            // Death specific function
            if (func != null)
            {
                func();
            }

            // if the enemy is already death, there is no point going further. this function end here.
            return;
        }

        if (hpbarHandle == null)
        {
            //instiate prefab
            hpbarHandle = Instantiate(hpbar);
            hpbarHandle.transform.SetParent(cam.transform.GetChild(0), false);
            hpbarHandle.SetActive(true);
            //set position
            Vector3 barPos = transform.position;
            barPos.y += hpBarOffset;
            hpbarHandle.transform.position = cam.WorldToScreenPoint(barPos);
            hpbarHandle.GetComponent<CanvasProperties>().SetTextMesh(0, enemyName);
            hpbarHandle.GetComponent<CanvasProperties>().SetAlpha(0.85f, hpBarDisplaySpeed, hpBarDisplayTime);
            hpbarHandle.GetComponent<CanvasProperties>().SetProgressorColor(1, hpBarFollowColor);
            hpbarHandle.GetComponent<CanvasProperties>().SetScaleX(triggerCollider.bounds.size.x * hpBarLengthFormula);
        }
        else
        {
            hpbarHandle.GetComponent<CanvasProperties>().SetAlpha(0.85f, hpBarDisplaySpeed, hpBarDisplayTime);
        }
    }

    public float GetCurrentHitPoint()
    {
        return hpcurrent;
    }

    private bool IsObjectInSight(Transform transform, Camera cam)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1
            && viewPos.y >= 0 && viewPos.y <= 1
            && viewPos.z > 0)
        {
            return true;
        }
        return false;
    }
}
