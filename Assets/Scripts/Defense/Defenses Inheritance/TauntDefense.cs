using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntDefense : Defense
{
    [Header("Taunt Defense Settings")]
    public float retaliatoryDamage = 10f;

    private Damageable damageable;

    protected override void Start()
    {
        base.Start();
        damageable = GetComponent<Damageable>();

        if (damageable == null)
        {
            Debug.LogError($"Damageable component missing on {gameObject.name}!");
        }
    }

    public void ApplyRetaliationDamage(GameObject attacker)
    {
        if (attacker == null) return;

        Damageable enemyDamageable = attacker.GetComponent<Damageable>();

        if (enemyDamageable != null)
        {
            Debug.Log($"{gameObject.name} retaliates against {attacker.name} for {retaliatoryDamage} damage!");
            enemyDamageable.ApplyDamage(retaliatoryDamage);
        }
    }
}