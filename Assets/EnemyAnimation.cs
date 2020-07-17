using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Animator animator;
    public LayerMask layerMask;

    public float speed = 0.001f;
    public float frontSpeedMultiplier = 1.25f;
    public float animationTransmitionRate = 5.0f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothVelocity;

    float horizontal;
    float vertical;
    float moveSpeed;    //speed after multiplier this frame
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", 0.0f);
        //animator.SetTrigger("damage");
    }
}
