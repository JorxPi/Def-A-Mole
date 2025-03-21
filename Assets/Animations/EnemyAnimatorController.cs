using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    private Animator animator;
    private EnemyAI enemyAI;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {

    }

    public void PlaySpawn() 
    {
        animator.SetBool("isSpawning", true);
        animator.SetBool("isWalking", false);
    }

    public void PlayWalk()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);
    }

    public void PlayAttack()
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
    }

    public void PlayDead()
    {
        animator.SetBool("isDead", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }
}
