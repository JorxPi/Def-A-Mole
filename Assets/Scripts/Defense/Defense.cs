using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all defenses
public class Defense : MonoBehaviour
{
    [Header("Defense Stats")]
    public float attackRange = 5f;
    public float attackDamage = 20f;
    public float attackSpeed = 1f;
    public int deployCost = 20;

    protected float attackCooldown = 0f;

    protected virtual void Start()
    {
        // Base initialization
    }

    protected virtual void Update()
    {
        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
    }

    public bool CanAttack()
    {
        return attackCooldown <= 0f;
    }

    public void ResetAttackCooldown()
    {
        attackCooldown = 1f / attackSpeed;
    }
}
