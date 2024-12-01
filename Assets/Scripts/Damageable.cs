using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health;

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
