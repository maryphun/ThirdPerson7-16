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
            target.GetComponent<Rigidbody>().AddForce(Vector3.up * 3f, ForceMode.Impulse);
            hittedEnemy.Add(target.transform);
            if (characterAttack != null)
            {
                characterAttack.AttackHitted(true, target.attachedRigidbody.mass);
            }
        }
    }

    private void DealDamage(Transform target)
    {
        //deal damage
        float damageCalculate = Random.Range(attackDamageMin, attackDamageMax);
        if (target.GetComponent<EnemyAnimation>() != null)
        {
            target.GetComponent<EnemyAnimation>().TakeDamage(damageCalculate);
        }

        //draw fx
        GameObject hitEffect = Instantiate(hitFX, target.transform.position, Quaternion.identity);
        Destroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.duration);
    }
}
