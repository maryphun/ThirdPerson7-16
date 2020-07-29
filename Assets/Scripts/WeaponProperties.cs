using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SPECIAL_EFFECT
{
    None,
    Lightning,
    Explosion,
}

public class WeaponProperties : MonoBehaviour
{
    public enum WEAPON_TYPE
    {
        HeavySword,
        LightSword,
        Axe,
        UltraHeavyProps,
    }
    
    public BoxCollider collider;
    public Cinemachine.CharacterAttack characterAttack;
    public WEAPON_TYPE weaponType;
    public SPECIAL_EFFECT weaponCode = SPECIAL_EFFECT.None;

    public float weaponMass = 1.0f;
    public float attackDamageMin = 0.0f;
    public float attackDamageMax = 0.0f;
    public GameObject hitFX;


    List<Transform> hittedEnemy = new List<Transform>();

    public void ColliderSwitch(bool boolean)
    {
        collider.enabled = boolean;
        hittedEnemy.Clear();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Enemy" && !hittedEnemy.Contains(target.transform))
        {
            hittedEnemy.Add(target.transform);
            if (characterAttack != null)
            {
                characterAttack.AttackHitted(true, target.attachedRigidbody.mass);
            }
            DealDamage(target.transform);
        }
    }

    private void DealDamage(Transform damagedtarget)
    {
        //deal damage
        float damageCalculate = Random.Range(attackDamageMin, attackDamageMax);
        if (damagedtarget.GetComponent<EnemyProperties>() != null)
        {
            damagedtarget.GetComponent<EnemyProperties>().TakeDamage(damageCalculate);
        }

        //draw fx
        if (hitFX != null)
        {
            Vector3 damagePosition = damagedtarget.transform.position;
            damagePosition.y = transform.Find("ImpactPoint").transform.position.y;
            GameObject fx = Instantiate(hitFX, damagePosition, Quaternion.identity);
        }
    }
}
