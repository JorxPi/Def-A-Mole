using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health;
    /*private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider collider)
    {
        Debug.Log("attacker");
        // Check if the collider belongs to a damage source (e.g., projectile)
        DamageSource damageSource = collider.GetComponent<DamageSource>();
        if (damageSource != null)
        {
            Debug.Log("attacker2");
            MonoBehaviour attacker = damageSource.GetOwner();
            if (attacker != null)
            {
                Debug.Log("attacker3");
                Debug.Log(attacker);
                ApplyDamage(attacker);
            }
        }
    }*/

    public void ApplyDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
            
        
    }

    private void Die()
    {
        Destroy(gameObject); 
    }
}
