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

        StartCoroutine(FlashRedEffect());

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

    private IEnumerator FlashRedEffect()
    {
        // Store original colors
        Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

        // Get all MeshRenderers and SkinnedMeshRenderers
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogError($"No Renderer found on {gameObject.name}");
            yield break;
        }

        // Store original colors and apply red color
        foreach (Renderer renderer in renderers)
        {
            originalColors[renderer] = renderer.material.color;
            renderer.material.color = Color.red;
        }

        // Wait for 0.2 seconds
        yield return new WaitForSeconds(0.2f);

        // Restore original colors
        foreach (Renderer renderer in renderers)
        {
            if (originalColors.ContainsKey(renderer))
            {
                renderer.material.color = originalColors[renderer];
            }
        }
    }
}
