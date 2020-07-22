using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [Header("Object Parameters")]
    public Animator animator;

    float AttackCombo;

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void DamageFrame(int combo)
    {
        Debug.Log("Deal Damge combo " + combo);
    }
}
