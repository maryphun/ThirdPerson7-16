using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Animator animator;
    public LayerMask IKLayerMask;

    public float speed = 0.001f;
    public float frontSpeedMultiplier = 1.25f;
    public float animationTransmitionRate = 5.0f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothVelocity;

    float horizontal;
    float vertical;
    float moveSpeed;    //speed after multiplier this frame

    [Range (0, 1f)]
    public float distanceToGround;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", 0.0f);
        //animator.SetTrigger("damage");
        rigidbody.velocity = Vector3.zero;
    }

    public void TakeDamage(float damage)
    {

    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            //Left Foot
            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, IKLayerMask))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distanceToGround;
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }

            //Right Foot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, distanceToGround + 1f, IKLayerMask))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distanceToGround;
                animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }
        }
    }
}
