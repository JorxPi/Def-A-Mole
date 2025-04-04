using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayAttack()
    {
        animator.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    public void PlayDead()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
    }
}

