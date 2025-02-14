using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Damageable damageable;
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
