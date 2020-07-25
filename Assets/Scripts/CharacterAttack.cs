using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class CharacterAttack : MonoBehaviour
    {
        [Header("Object Parameters")]
        public Animator animator;
        public Camera camera;
        public ThirdPersonControl ThirdPersonControl;
        public Transform Weapon;
        public GameObject[] effectFX;


        [Header("Float Parameters")]

        public CinemachineImpulseDefinition m_ImpulseDefinition = new CinemachineImpulseDefinition();
        public CinemachineImpulseDefinition m_ImpulseDefinitionNoise = new CinemachineImpulseDefinition();


        float AttackCombo;
        bool attackHitted;

        public void Attack()
        {
            animator.SetTrigger("Attack");
        }

        public void DamageFrameStart(int combo)
        {
            AttackCombo = combo;
            attackHitted = false;
            if (Weapon != null)
            {
                Weapon.GetComponent<WeaponProperties>().ColliderSwitch(true);
            }
        }

        public void DamageFrameEnd(int combo)
        {
            AttackCombo = combo;
            if (Weapon != null)
            {
                Weapon.GetComponent<WeaponProperties>().ColliderSwitch(false);
            }
        }

        public void CameraShake(float magnitude)
        {
            if (m_ImpulseDefinition != null)
            {
                m_ImpulseDefinition.CreateEvent(transform.position, Vector3.down * magnitude);
            }
        }

        public void CameraShakeNoise(float magnitude)
        {
            if (m_ImpulseDefinitionNoise != null)
            {
                m_ImpulseDefinitionNoise.CreateEvent(transform.position, Vector3.down * magnitude);
            }
        }

        
        public void MoveFront(float distance)
        {

            float moveTargetAngle = Mathf.Atan2(0.0f, 1.0f) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;
            float moveSpeed = distance;
            if (Weapon != null)
            {
                moveSpeed *= Weapon.GetComponent<WeaponProperties>().weaponMass;
            }

            //condition before everything so it won't waste resource to throw a ray
            if (ThirdPersonControl.TooCloseToLockOnTarget(transform.position + moveDirection * moveSpeed * Time.deltaTime) || attackHitted)
            {
                return;
            }

            ///CollideCheck Rasycast
            RaycastHit hit;
            RaycastHit hit2;
            Vector3 origin = transform.position;
            origin += Vector3.up * ThirdPersonControl.climbHeightMaximum;
            Vector3 end;

            end = origin + (moveDirection * moveSpeed * ThirdPersonControl.collisionCheckRange);
            if (!Physics.Raycast(origin, moveDirection, out hit, moveSpeed * ThirdPersonControl.collisionCheckRange, ThirdPersonControl.layerMask))
            {
                if (!Physics.Raycast(end, Vector3.up, out hit2, 0.5f, ThirdPersonControl.layerMask))
                {
                    this.GetComponent<Rigidbody>().AddForce(moveDirection * moveSpeed, ForceMode.Impulse);
                }
            }
        }

        public void WeaponTrailActivate(int child)
        {
            if (Weapon != null)
            {
                Weapon.Find("Trail").gameObject.SetActive(true);
            }
        }
        
        public void WeaponTrailDeactivate(int child)
        {
            if (Weapon != null)
            {
                Weapon.Find("Trail").gameObject.SetActive(false);
            }
        }
        
        //Call from other script (Weapon Collider)
        public void AttackHitted(bool boolean, float enemyMass)
        {
            attackHitted = boolean;

            //reset force
            if (attackHitted)
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                float moveTargetAngle = Mathf.Atan2(0.0f, 1.0f) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                Vector3 moveDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;
                float weaponMass = 1.0f; //default
                if (Weapon != null)
                {
                    weaponMass = Weapon.GetComponent<WeaponProperties>().weaponMass;
                }
                this.GetComponent<Rigidbody>().AddForce((-moveDirection * enemyMass) * weaponMass, ForceMode.Impulse);
            }
        }

        //instantiate fx at position of player
        public void PlayFX(int variable)
        {
            GameObject particleSystem = Instantiate(effectFX[variable], transform.position, Quaternion.identity);
            Destroy(particleSystem, particleSystem.GetComponent<ParticleSystem>().main.duration);
        }

        //instantiate fx at weapon
        public void PlayFXWeaponHitGround(int variable)
        {
            Vector3 position = transform.position;
            if (Weapon != null)
            {
                Transform weaponImpactPoint = Weapon.Find("ImpactPoint").transform;

                if (weaponImpactPoint != null)
                {
                    position.x = weaponImpactPoint.position.x;
                    position.z = weaponImpactPoint.position.z;
                }
            }

            GameObject particleSystem = Instantiate(effectFX[variable], position, Quaternion.identity);
            Destroy(particleSystem, particleSystem.GetComponent<ParticleSystem>().main.duration);
        }

        private void OnValidate()
        {
            m_ImpulseDefinition.OnValidate();
        }
    }
}