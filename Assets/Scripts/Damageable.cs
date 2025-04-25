using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Damageable : MonoBehaviour
{
    public float health;
    [HideInInspector]
    public float maxHealth;

    [Header("Optional")]
    public Image healthBar;

    private Defense defense;
    private bool isDead = false;

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>(); //(stores original color to change back after the red effect)


    public event Action OnDeath;

    private void Awake()
    {
        defense = GetComponent<Defense>();
        if (defense != null && defense.baseStats != null)
        {
            maxHealth = defense.baseStats.maxHealth;
            health = maxHealth; 
        }

        UpdateHealthBar();

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

        UpdateHealthBar();

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
        if (isDead) return; 
        isDead = true;

        OnDeath?.Invoke();

        Debug.Log("Die-Damageable");
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

            enemy.enemyAI.Die();

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = true;
                agent.enabled = false;
            }

            EnemyAI ai = GetComponent<EnemyAI>();
            if (ai != null) ai.enabled = false;

            gameObject.tag = "Untagged";
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        Destroy(gameObject, 2f);
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

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (Renderer renderer in renderers)
        {
            if (originalColors.TryGetValue(renderer, out Color originalColor))
            {
                renderer.material.color = originalColor;
            }
        }
    }
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float normalized = Mathf.Clamp01(health / maxHealth);
            healthBar.fillAmount = normalized;
        }
    }
}
