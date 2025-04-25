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
        animator.ResetTrigger("attack"); 
        animator.SetTrigger("attack");
    }

    /*public void StopAttack()
    {
        Debug.Log("StopAttack");
        animator.SetBool("isAttacking", false);
    }*/

    public void PlayDead()
    {
        animator.SetBool("isDead", true);
    }
}

