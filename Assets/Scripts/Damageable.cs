using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health;
    [HideInInspector]
    public float maxHealth;

    private Defense defense;

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>(); //(stores original color to change back after the red effect)


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

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (!originalColors.ContainsKey(renderer))
            {
                originalColors[renderer] = renderer.material.color;
            }
        }
    }

    public void ApplyDamage(float damage, GameObject attacker = null)
    {
        health -= damage;

        StartCoroutine(FlashRedEffect());

        // !! ONLY FOR TAUNTDEFENSE !!
        TauntDefense tauntDefense = GetComponent<TauntDefense>();
        if (tauntDefense != null && attacker != null)
        {
            tauntDefense.ApplyRetaliationDamage(attacker);
        }
        // !! ONLY FOR TAUNTDEFENSE !!

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
            if (vignetteController != null && enemy.enemyAI.isAttackingBarrier)
            {
                vignetteController.DecreaseEnemiesAttacking();
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

    private IEnumerator FlashRedEffect()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogError($"No Renderer found on {gameObject.name}");
            yield break;
        }

        // Apply red color
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.red;
        }

        // Wait for 0.2 seconds
        yield return new WaitForSeconds(0.2f);

        // Restore the original color from the stored dictionary
        foreach (Renderer renderer in renderers)
        {
            if (originalColors.TryGetValue(renderer, out Color originalColor))
            {
                renderer.material.color = originalColor;
            }
        }
    }
}
