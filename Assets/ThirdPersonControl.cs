using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdPersonControl : MonoBehaviour
{
    public Transform camera;
    public Rigidbody rigidbody;
    public Animator animator;

    public float speed = 0.001f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log(horizontal.ToString() + vertical.ToString());

        float targetAngle = /*Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + */camera.eulerAngles.y;
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
       // transform.GetChild(0).localRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.localRotation = Quaternion.Euler(0f, targetAngle, 0f);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float moveTargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            //Vector3 moveVector = new Vector3(0f, moveTargetAngle, 0f) + Vector3.forward;
            //rigidbody.AddForce(moveDirection * speed * Time.deltaTime, ForceMode.VelocityChange);
            transform.DOBlendableLocalMoveBy(moveDirection * speed * Time.deltaTime, 0f);
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(Vector3.up * 3.5f, ForceMode.Impulse);
        }
    }
}
