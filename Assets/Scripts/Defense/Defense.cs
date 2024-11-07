using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//will be abstract
public class Defense : MonoBehaviour
{
    [Header("Defense Stats")]
    public float attackRange = 5f;
    public float attackDamage = 20f;
    public float attackSpeed = 1f;
    public int deployCost = 100;

    protected float attackCooldown = 0f;

    // will be virtual
    protected void Start()
    {
        
    }

    // virtual
    protected void Update()
    {
        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
    }

    // Method to be called by Brain for attacking
    public bool CanAttack()
    {
        return attackCooldown <= 0f;
    }

    public void ResetAttackCooldown()
    {
        attackCooldown = 1f / attackSpeed;
    }
}