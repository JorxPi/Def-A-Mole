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
    public float health = 100f;

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

    //have to change this
    /*public void ApplyDamage(MonoBehaviour attacker)
    {
        if (attacker is Enemy enemy)
        {
            float damage = enemy.attackDamage; // Use the enemy's attack damage
            health -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage from {enemy.name}. Remaining health: {health}");

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed.");
        Destroy(gameObject); // Handle any additional cleanup here
    }*/
}
