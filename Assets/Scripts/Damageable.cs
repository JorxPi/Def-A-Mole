using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health;
    public float maxHealth;

    private Defense defense;


    //Event used for gold update on enemy death
    public event Action OnDeath;

    private void Awake()
    {
        defense = GetComponent<Defense>();
        if (defense != null && defense.baseStats != null)
        {
            maxHealth = defense.baseStats.maxHealth;
            health = maxHealth; 
        }
    }

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

        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            DamageVignetteController vignetteController = FindObjectOfType<DamageVignetteController>();
            if (vignetteController != null)
            {
                vignetteController.OnEnemyDeath(gameObject);
            }
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levelManager.EnemyDied();
        }

        Destroy(gameObject);
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        health += amount; 
        health = Mathf.Min(health, maxHealth);
    }
}
