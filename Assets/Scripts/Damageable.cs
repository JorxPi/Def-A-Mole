using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Damageable : MonoBehaviour
{
    public float health;
    [HideInInspector] public float maxHealth;

    [Header("Optional")]
    public Image healthBar;

    private Defense defense;
    private bool isDead = false;

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    public event Action OnDeath;

    // --- AHORA PÚBLICOS PARA ASIGNAR EN EL INSPECTOR ---
    [Header("Particle Effects")]
    public GameObject smokeEffect;
    public GameObject fireEffect;
    // ----------------------------------------------------

    private bool smokeActive = false;
    private bool fireActive = false;

    private void Awake()
    {
        defense = GetComponent<Defense>();
        if (defense != null && defense.baseStats != null)
        {
            maxHealth = defense.baseStats.maxHealth;
            health = maxHealth;
        }
        else
        {
            maxHealth = health;
        }

        UpdateHealthBar();

        // Guardamos colores originales
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            if (!originalColors.ContainsKey(renderer))
                originalColors[renderer] = renderer.material.color;

        // Desactivamos efectos al inicio (si están asignados)
        if (smokeEffect != null) smokeEffect.SetActive(false);
        if (fireEffect != null) fireEffect.SetActive(false);
    }

    public void ApplyDamage(float damage, GameObject attacker = null)
    {
        health -= damage;
        UpdateHealthBar();
        StartCoroutine(FlashRedEffect());

        // !! ONLY FOR TAUNTDEFENSE !!
        var taunt = GetComponent<TauntDefense>();
        if (taunt != null && attacker != null)
            taunt.ApplyRetaliationDamage(attacker);
        // !! ONLY FOR TAUNTDEFENSE !!

        UpdateDamageEffects();

        if (health <= 0f)
            Die();
    }

    private void UpdateDamageEffects()
    {
        float pct = health / maxHealth;

        if (pct <= 0.2f) // 20%  fuego
        {
            if (!fireActive)
            {
                if (smokeEffect != null) smokeEffect.SetActive(false);
                if (fireEffect != null) fireEffect.SetActive(true);
                fireActive = true;
                smokeActive = false;
            }
        }
        else if (pct <= 0.6f) // 60%  humo
        {
            if (!smokeActive)
            {
                if (fireEffect != null) fireEffect.SetActive(false);
                if (smokeEffect != null) smokeEffect.SetActive(true);
                smokeActive = true;
                fireActive = false;
            }
        }
        else // 60%  ninguno
        {
            if (smokeActive || fireActive)
            {
                if (smokeEffect != null) smokeEffect.SetActive(false);
                if (fireEffect != null) fireEffect.SetActive(false);
                smokeActive = fireActive = false;
            }
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        OnDeath?.Invoke();
        Debug.Log("Die-Damageable");

        if (GetComponent<Defense>() != null)
        {
            AudioManager.Instance.PlayDefenseDestroyed(0.1f);
        }

        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            var vignette = FindObjectOfType<DamageVignetteController>();
            if (vignette != null && enemy.enemyAI.isAttackingBarrier)
                vignette.DecreaseEnemiesAttacking();

            FindObjectOfType<LevelManager>()?.EnemyDied();

            enemy.enemyAI.Die();

            foreach (var col in GetComponentsInChildren<Collider>())
                col.enabled = false;

            var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = true;
                agent.enabled = false;
            }

            var ai = GetComponent<EnemyAI>();
            if (ai != null) ai.enabled = false;

            gameObject.tag = "Untagged";
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        Destroy(gameObject, 2f);
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        health = Mathf.Min(health + amount, maxHealth);
        UpdateHealthBar();
    }

    private IEnumerator FlashRedEffect()
    {
        int indicatorLayer = LayerMask.NameToLayer("Indicator");

        var renderers = GetComponentsInChildren<Renderer>()
                         .Where(r => r.gameObject.layer != indicatorLayer)
                         .ToArray();

        if (renderers.Length == 0)
            yield break;

        foreach (var r in renderers)
        {
            if (!originalColors.ContainsKey(r))
                originalColors[r] = r.material.color;
        }

        foreach (var r in renderers)
            r.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        foreach (var r in renderers)
            if (originalColors.TryGetValue(r, out var col))
                r.material.color = col;
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
    }
}
