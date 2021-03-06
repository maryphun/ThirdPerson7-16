using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIKFootPlacement : MonoBehaviour
{
    public Animator animator;
    public LayerMask IKLayerMask;

    [Range(0, 1f)]
    public float distanceToGround;

    void OnAnimatorIK()
    {
        if (animator)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.7f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.7f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.7f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.7f);

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
