using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public Damageable damageable;
    [HideInInspector]
    public EnemyAI enemyAI;

    public int goldReward = 1;

    protected virtual void Start()
    {
        // Inicializar componentes
        damageable = GetComponent<Damageable>();
        enemyAI = GetComponent<EnemyAI>();
        

        // Configurar stats iniciales
        if (damageable != null)
        {
            damageable.OnDeath += HandleDeath;
        }
        
    }

    protected virtual void HandleDeath()
    {
        if (enemyAI.isAttackingBarrier)
        {
            DamageVignetteController vignetteController = FindObjectOfType<DamageVignetteController>();
            vignetteController.DecreaseEnemiesAttacking();
        }
        CurrencyManager.Instance.AddGold(goldReward);
        //Destroy(gameObject, 2f);
    }

    private void OnDestroy()
    {
        if (damageable != null)
        {
            damageable.OnDeath -= HandleDeath;
        }
    }
}
