using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHealth = 100f;
    public float speed = 2f;
    public float damageAmount = 10f;
    public int goldReward = 100;

    public Damageable damageable;
    public EnemyAI enemyAI;

    protected virtual void Start()
    {
        // Inicializar componentes
        damageable = GetComponent<Damageable>();
        enemyAI = GetComponent<EnemyAI>();
        
        // Configurar stats iniciales
        if (damageable != null)
        {
            damageable.maxHealth = maxHealth;
            damageable.OnDeath += HandleDeath;
        }
        
        if (enemyAI != null)
        {
            enemyAI.damageAmount = damageAmount;
        }
    }

    protected virtual void HandleDeath()
    {
        CurrencyManager.Instance.AddGold(goldReward);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (damageable != null)
        {
            damageable.OnDeath -= HandleDeath;
        }
    }
}
