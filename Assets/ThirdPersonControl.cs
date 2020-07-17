using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdPersonControl : MonoBehaviour
{
    public Transform camera;
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
        moveSpeed = speed; // set to default

        horizontal = Mathf.MoveTowards(horizontal, Input.GetAxisRaw("Horizontal") * 100f, animationTransmitionRate);
        vertical = Mathf.MoveTowards(vertical, Input.GetAxisRaw("Vertical") * 100f, animationTransmitionRate);
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            moveSpeed *= frontSpeedMultiplier;
        }
        //Debug.Log("Horizontal:" + horizontal.ToString()+ ",Vertical:" + vertical.ToString());

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Speed", speed* 0.75f);
        animator.SetBool("Idle", (horizontal == 0 && vertical == 0));
        //Debug.Log((horizontal == 0 && vertical == 0));

        float targetAngle = /*Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + */camera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
       // transform.GetChild(0).localRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.localRotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float moveTargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            //Vector3 moveVector = new Vector3(0f, moveTargetAngle, 0f) + Vector3.forward;
            //rigidbody.AddForce(moveDirection * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            ///CollideCheck Rasycast
            RaycastHit hit;
            RaycastHit hit2;
            Vector3 origin = transform.position;
            origin += Vector3.up * 0.25f;

            Vector3 end;
            end = origin + (moveDirection * moveSpeed * 0.15f);
            Debug.Log("(" + moveDirection.x + "," + moveDirection.y + "," + moveDirection.z + ")");
            if (Physics.Raycast(origin, moveDirection, out hit, moveSpeed * 0.15f, layerMask))
            {
                Debug.DrawLine(origin, end, Color.red);
            }
            else
            {
                Debug.DrawLine(origin, end, Color.green);
                if (Physics.Raycast(end, Vector3.up, out hit2, 0.5f, layerMask))
                {
                    Debug.DrawLine(end, end + Vector3.up * 0.5f, Color.red);
                }
                else
                {
                    transform.DOBlendableLocalMoveBy(moveDirection * moveSpeed * Time.deltaTime, 0f);
                    Debug.DrawLine(end, end + Vector3.up * 0.5f, Color.green);
                }
            }
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(Vector3.up * 3.5f, ForceMode.Impulse);
        }
    }
}
