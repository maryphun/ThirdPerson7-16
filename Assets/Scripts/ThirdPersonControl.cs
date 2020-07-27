﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Cinemachine.Utility;

public class ThirdPersonControl : MonoBehaviour
{
    [Header("Object Parameters")]
    public Camera camera;
    public Rigidbody rigidbody;
    public Animator animator;
    public LayerMask layerMask;
    public Transform lockOnTarget;
    public CinemachineVirtualCamera lockOnCamera;
    public CinemachineFreeLook freeLookCamera;
    public GameObject Reticle;
    public CharacterAttack AttackScript;
    public GameObject WalkDustLeft;
    public GameObject WalkDustRight;

    [Header("Float Parameters")]
    public float speed = 0.001f;
    public float frontSpeedMultiplier = 1.25f;
    public float horizontalSpeedMultiplier = 1.25f;
    public float rollingSpeed = 3f;
    public float rollingTime = 1.0f;
    public float animationTransmitionRate = 5.0f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothVelocity;
    public float lockOnSearchRange = 20.0f;
    public float climbHeightMaximum = 0.25f;
    public float YaxisClimbLerpDelay = 3;   //The higher the value the smoothier it is but take more memory
    public float lockOnCameraOffset = 0.15f;
    public float collisionCheckRange = 0.10f;

    [Header("Key Input Customization")]
    public KeyCode Rolling = KeyCode.Space;
    public KeyCode LockOn = KeyCode.Mouse2;
    public KeyCode CameraSideLeft = KeyCode.Q;
    public KeyCode CameraSideRight = KeyCode.E;
    public KeyCode Attack = KeyCode.Mouse0;
    public KeyCode WeaponSwitchKey = KeyCode.F;

    float horizontal;
    float vertical;
    float moveSpeed;    //speed after multiplier this frame
    float rollingDelta;
    bool lockOnMode = false;
    bool isRolling;
    Vector3 rollDirection;
    CinemachineOrbitalTransposer[] orbital = new CinemachineOrbitalTransposer[3];
    CinemachineVirtualCamera[] rigs = new CinemachineVirtualCamera[3];

    void Start()
    {
        //initialization
        lockOnCamera.Priority = 0;
        freeLookCamera.Priority = 1;
        lockOnMode = false;

        for (int i = 0; freeLookCamera != null && i < 3; ++i)
        {
            rigs[i] = freeLookCamera.GetRig(i);
            orbital[i] = rigs[i].GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }
    }
    
    void Update()
    {
        moveSpeed = speed; // set to default

        //input
        if (!isRolling)
        {
            horizontal = Mathf.MoveTowards(horizontal, Input.GetAxisRaw("Horizontal") * 100f, animationTransmitionRate);
            vertical = Mathf.MoveTowards(vertical, Input.GetAxisRaw("Vertical") * 100f, animationTransmitionRate);
            if (Input.GetKeyDown(Rolling)) Roll();
        }
        if (Input.GetKeyDown(LockOn)) //middle click
        {
            LockOnTriggered();
        }
        if (lockOnMode)
        {
            ChangeSideCamera(Input.GetKeyDown(CameraSideLeft), Input.GetKeyDown(CameraSideRight));
        }
        if (Input.GetKeyDown(Attack))
        {
            AttackScript.Attack();
        }
        if (Input.GetKeyDown(WeaponSwitchKey))
        {
            SwitchWeapon();
        }

        //Speed Modifier
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            moveSpeed *= frontSpeedMultiplier;
        }
        if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            moveSpeed *= frontSpeedMultiplier;
        }

        //animation parameters
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Speed", speed * 0.75f);
        animator.SetBool("Idle", (horizontal == 0 && vertical == 0));

        //rotate
        if (lockOnMode)
        {
            Vector3 targetPostition = new Vector3(lockOnTarget.position.x, this.transform.position.y, lockOnTarget.position.z);
            transform.DOLookAt(targetPostition, turnSmoothTime);
        }
        else
        {
            //freelook camera
            float targetAngle = camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        //jump
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rigidbody.AddForce(Vector3.up * 3.5f, ForceMode.Impulse);
        //}

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //move
        if ((direction.magnitude >= 0.1f || isRolling) && 
            (animator.GetCurrentAnimatorStateInfo(0).IsName("Rolling Locomotion") || animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")))
        {
            Vector3 moveDirection;
            //Rolling Replacement
            if (!isRolling)
            {
                float moveTargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                moveDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;
            }
            else
            {
                //replace movespeed
                moveSpeed = RollSpeed(rollingDelta);
                //replace direction
                moveDirection = rollDirection;
                //check if it should be ended

                rollingDelta -= Time.deltaTime;
                if (rollingDelta <= 0.0f)
                {
                    isRolling = false;
                    animator.SetTrigger("RollEnd");
                }
            }

            ///CollideCheck Rasycast
            RaycastHit hit;
            RaycastHit hit2;
            Vector3 origin = transform.position;
            origin += Vector3.up * climbHeightMaximum;

            Vector3 end;
            end = origin + (moveDirection * moveSpeed * collisionCheckRange);
            if (Physics.Raycast(origin, moveDirection, out hit, moveSpeed * collisionCheckRange, layerMask))
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
                    Move(moveDirection, moveSpeed, 0.0f);
                    Debug.DrawLine(end, end + Vector3.up * 0.5f, Color.green);

                    //dust particle
                    ParticleSystem.EmissionModule emissionModule = WalkDustLeft.GetComponent<ParticleSystem>().emission;
                    if (!emissionModule.enabled)
                    {
                        emissionModule.enabled = true;
                        emissionModule = WalkDustRight.GetComponent<ParticleSystem>().emission;
                        emissionModule.enabled = true;
                    }
                }
            }
        }
        else if (WalkDustLeft.activeSelf)
        {
            //turn off dust particle when you're not walking
            ParticleSystem.EmissionModule emissionModule = WalkDustLeft.GetComponent<ParticleSystem>().emission;
            if (emissionModule.enabled)
            {
                emissionModule.enabled = false;
                emissionModule = WalkDustRight.GetComponent<ParticleSystem>().emission;
                emissionModule.enabled = false;
            }
        }
    }

    Transform FindTarget()
    {
        Transform rtn = null;
        GameObject[] enemies;
        List<GameObject> enemiesInSight = new List<GameObject>();
        float distanceCompare = lockOnSearchRange;

        //search enemy
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                //Check if it's in sight, and not too close to the player
                if (IsObjectInSight(enemy.transform, camera) && Vector3.Distance(enemy.transform.position, this.transform.position) > speed /2)
                {
                    enemiesInSight.Add(enemy);
                }
            }
        }

        //compare all enemy in sight, get the nearest one
        if (enemiesInSight != null)
        {
            foreach (GameObject enemy in enemiesInSight)
            {
                float newDistance = Vector3.Distance(enemy.transform.position, this.transform.position);
                if (distanceCompare > newDistance)
                {
                    distanceCompare = newDistance;
                    //finally get the target
                    rtn = enemy.transform;
                }
            }
        }

        //clear everything
        enemies = null;
        enemiesInSight.Clear();

        return rtn;
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

    void LockOnTriggered()
    {
        if (!lockOnMode)
        {
            //change to lock on mode if the camera can find a target
            lockOnTarget = FindTarget();
            if (lockOnTarget != null)
            {
                lockOnCamera.Priority = 1;
                freeLookCamera.Priority = 0;
                lockOnMode = true;

                //set aiming target
                lockOnCamera.LookAt = GetCameraFocusAvailable(lockOnTarget);
            }
        }
        else
        {
            //back to default
            lockOnCamera.Priority = 0;
            freeLookCamera.Priority = 1;
            lockOnCamera.LookAt = null;
            lockOnTarget = null;
            lockOnMode = false;

            //reset free look camera's rotation
            Transform target = freeLookCamera != null ? freeLookCamera.Follow : null;
            if (target == null)
                return;
        }

        //Canvas
        Reticle.SetActive(lockOnMode);
    }

    bool ChangeSideCamera(bool leftside, bool rightside)
    {
        if (leftside)
        {
            lockOnCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f - lockOnCameraOffset;
            return true;
        }
        else if (rightside)
        {
            lockOnCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f + lockOnCameraOffset;
            return true;
        }
        return false;
    }

    public void Move(Vector3 moveDirection, float range, float delay)
    {
        //check Y of new position
        RaycastHit hit;
        Vector3 origin = transform.position + (moveDirection * range * collisionCheckRange);
        origin += Vector3.up * climbHeightMaximum;
        bool GroundHit = (Physics.Raycast(origin, Vector3.down, out hit, climbHeightMaximum, layerMask));
        
        if (GroundHit)
        {
            Debug.DrawLine(transform.position, new Vector3(hit.point.x, transform.position.y, hit.point.z), Color.cyan, 2f);
            float damping = YaxisClimbLerpDelay;
            //move Y smoothly depends on the length difference
            transform.DOMoveY(hit.point.y, (hit.point.y - transform.position.y) * damping);
        }
        else
        {
            Debug.DrawLine(origin, origin + new Vector3(0.0f, -climbHeightMaximum, 0.0f), Color.blue);
            Debug.DrawLine(transform.position, new Vector3(origin.x, transform.position.y, origin.z), Color.white);
        }

        //move XZ
        if (!TooCloseToLockOnTarget(transform.position + moveDirection * range * Time.deltaTime))
        {
            transform.DOBlendableLocalMoveBy(moveDirection * range * Time.deltaTime, delay);
        }
    }

    Transform GetCameraFocusAvailable(Transform parent)
    {
        Transform rtn = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.tag == "CameraFocus")
            {
                return parent.GetChild(i).gameObject.transform;
                break;
            }
        }
        return rtn;
    }

    void Roll()
    {
        if (horizontal == 0 && vertical == 0)
        {
            //default roll direction
            //rollVertical = 100;
        }
        horizontal = Mathf.MoveTowards(horizontal, Input.GetAxisRaw("Horizontal") * 100f, 100f);
        vertical = Mathf.MoveTowards(vertical, Input.GetAxisRaw("Vertical") * 100f, 100f);

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        float moveTargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        rollDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;

        animator.SetTrigger("Roll");
        rollingDelta = rollingTime;
        isRolling = true;
    }

    float RollSpeed(float rolldelta)
    {
        return Mathf.Max(rollingSpeed / 1.5f, rollingSpeed * (rolldelta / rollingTime));
    }

    public bool TooCloseToLockOnTarget(Vector3 newPos)
    {
        bool rtn = false;
        
        if (lockOnMode && lockOnTarget != null)
        {
            float newDistance = Vector3.Distance(newPos, lockOnTarget.position);
            float oldDistance = Vector3.Distance(this.transform.position, lockOnTarget.position);
            if (newDistance < speed/2f && newDistance < oldDistance && horizontal == 0f)
            {
                return true;
            }
        }

        return rtn;
    }

    public void SwitchWeapon()
    {
        animator.SetTrigger("Equip");
    }
}