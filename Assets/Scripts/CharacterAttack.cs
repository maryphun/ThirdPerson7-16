using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class CharacterAttack : MonoBehaviour
    {
        [Header("Object Parameters")]
        public Animator animator;
        public Rigidbody rigibody;
        public Camera camera;
        public ThirdPersonControl ThirdPersonControl;
        public Transform weapon;
        public Transform secondWeapon;
        public Transform weaponParent;
        public Transform secondWeaponParent;
        public GameObject[] effectFX;


        [Header("Float Parameters")]

        public CinemachineImpulseDefinition m_ImpulseDefinition = new CinemachineImpulseDefinition();
        public CinemachineImpulseDefinition m_ImpulseDefinitionNoise = new CinemachineImpulseDefinition();


        float AttackCombo;
        bool attackHitted;

        private void Start()
        {
            //initiate weapon
            if (weapon != null)
            {
                //initiate animation set
                animator.SetInteger("WeaponType", ((int)weapon.GetComponent<WeaponProperties>().weaponType));

                //turn off collision of weapon by default
                weapon.GetComponent<WeaponProperties>().ColliderSwitch(false);
            }
            else
            {
                // no weapon
                animator.SetInteger("WeaponType", -1);
            }



        }

        public void Attack()
        {
            animator.SetTrigger("Attack");
        }

        public void DamageFrameStart(int combo)
        {
            AttackCombo = combo;
            attackHitted = false;
            if (weapon != null)
            {
                weapon.GetComponent<WeaponProperties>().ColliderSwitch(true);
            }
        }

        public void DamageFrameEnd(int combo)
        {
            AttackCombo = combo;
            if (weapon != null)
            {
                weapon.GetComponent<WeaponProperties>().ColliderSwitch(false);
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
            //reset last force
            rigibody.velocity = Vector3.zero;

            float moveTargetAngle = Mathf.Atan2(0.0f, 1.0f) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;
            float moveSpeed = distance;
            if (weapon != null)
            {
                moveSpeed /= weapon.GetComponent<WeaponProperties>().weaponMass;
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

        public void MoveUp(float distance)
        {
            // reset last force
            rigibody.velocity = Vector3.zero;

            // the heavier the weapon is the higher the player fly
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * distance * weapon.GetComponent<WeaponProperties>().weaponMass, ForceMode.Impulse);
        }

            public void WeaponTrailActivate(int child)
        {
            if (weapon != null)
            {
                weapon.Find("Trail").gameObject.SetActive(true);
                //ParticleSystem.EmissionModule particle = weapon.Find("Trail").GetChild(0).GetComponent<ParticleSystem>().emission;
                //particle.enabled = true;
            }
        }
        
        public void WeaponTrailDeactivate(int child)
        {
            if (weapon != null)
            {
                weapon.Find("Trail").gameObject.SetActive(false);
                //ParticleSystem.EmissionModule particle = weapon.Find("Trail").GetChild(0).GetComponent<ParticleSystem>().emission;
                //particle.enabled = false;
            }
        }
        
        //Call from other script (Weapon Collider)
        public void AttackHitted(bool boolean, float enemyMass)
        {
            attackHitted = boolean;
            float weaponMass = 1.0f; //default
            if (weapon != null)
            {
                weaponMass = weapon.GetComponent<WeaponProperties>().weaponMass;
            }

            //reset force
            if (attackHitted)
            {
                rigibody.velocity = Vector3.zero;
                float moveTargetAngle = Mathf.Atan2(0.0f, 1.0f) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                Vector3 moveDirection = Quaternion.Euler(0f, moveTargetAngle, 0f) * Vector3.forward;
                rigibody.AddForce((-moveDirection * enemyMass) / weaponMass, ForceMode.Impulse);
            }

            //shake the camera
            CameraShakeNoise(Mathf.Min(weaponMass, 0.4f));
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
            if (weapon != null)
            {
                Transform weaponImpactPoint = weapon.Find("ImpactPoint").transform;

                if (weaponImpactPoint != null)
                {
                    position.x = weaponImpactPoint.position.x;
                    position.z = weaponImpactPoint.position.z;
                }
            }

            GameObject particleSystem = Instantiate(effectFX[variable], position, Quaternion.identity);
            Destroy(particleSystem, particleSystem.GetComponent<ParticleSystem>().main.duration);
        }

        public void ChangeWeapon()
        {
            //store the two name so they could do exchange
            string onHandWeaponName = "";
            string secondWeaponName = "";

            // Check if the weapon is null
            if (weapon != null)
            {
                onHandWeaponName = weapon.name;
            }
            if (secondWeapon != null)
            {
                secondWeaponName = secondWeapon.name;
            }

            if (weapon != null && secondWeapon != null)
            {
                // Case 1 : Both Weapon aren't null
                weapon.gameObject.SetActive(false);
                secondWeapon.gameObject.SetActive(false);

                weapon = weaponParent.Find(secondWeaponName);
                secondWeapon = secondWeaponParent.Find(onHandWeaponName);
                
                weapon.gameObject.SetActive(true);
                secondWeapon.gameObject.SetActive(true);
            }
            else if (weapon != null && secondWeapon == null)
            {
                // Case 2 : No second weapon
                weapon.gameObject.SetActive(false);
                secondWeapon = secondWeaponParent.Find(onHandWeaponName);
                secondWeapon.gameObject.SetActive(true);
                weapon = null;
            }
            else if (weapon == null && secondWeapon != null)
            {
                // Case 3 : No on hand weapon
                secondWeapon.gameObject.SetActive(false);
                weapon = weaponParent.Find(secondWeaponName);
                weapon.gameObject.SetActive(true);
                secondWeapon = null;
            }

            // Get weapon properties to determine which attack animation to play on animator
            if (weapon != null)
            {
                animator.SetInteger("WeaponType", ((int)weapon.GetComponent<WeaponProperties>().weaponType));
            }
            else
            {
                // no weapon
                animator.SetInteger("WeaponType", -1);
            }

            // reset animation triggers
            foreach (AnimatorControllerParameter p in animator.parameters)
            {
                if (p.type == AnimatorControllerParameterType.Trigger)
                    animator.ResetTrigger(p.name);
            }
            //for (int i = 0; i <= animator.parameterCount; i++)
            //{
            //    //check if it's a trigger type parameters
            //    if (animator.GetParameter(i).GetType() == animator.GetParameter(4).GetType())
            //    {
            //        animator.ResetTrigger(i);
            //    }
            //}
        }

        void WeaponSpecificEffect()
        {
            switch (weapon.GetComponent<WeaponProperties>().weaponCode)
            {
                case SPECIAL_EFFECT.Lightning:
                    PlayFXWeaponHitGround(2);
                    break;
                default:
                    break;
            }
        }

        void ResetAnimation(string parameter)
        {
            animator.ResetTrigger(parameter);
        }

        


        private void OnValidate()
        {
            m_ImpulseDefinition.OnValidate();
        }
    }
}