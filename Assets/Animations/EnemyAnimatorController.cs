using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        if(animator == null)
         animator = GetComponentInChildren<Animator>();
    }

    public void PlayAttack()
    {
        Debug.Log("playttack");
        animator.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        Debug.Log("StopAttack");
        animator.SetBool("isAttacking", false);
    }

    public void PlayDead()
    {
        Debug.Log("die");
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
    }
}

