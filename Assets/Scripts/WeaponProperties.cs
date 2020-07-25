using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProperties : MonoBehaviour
{
    public enum WEAPON_TYPE
    {
        HeavySword,
        LightSword,
    }

    public BoxCollider collider;
    public Cinemachine.CharacterAttack characterAttack;
    public WEAPON_TYPE weaponType;

    public float weaponMass = 1.0f;
    

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
}
