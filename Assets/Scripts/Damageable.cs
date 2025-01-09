using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health;
    public float maxHealth; 

    //Event used for gold update on enemy death
    public event Action OnDeath;

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
        OnDeath?.Invoke();

        Destroy(gameObject); 
    }
}
