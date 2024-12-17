using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Damageable damageable;
    public int goldReward = 100;

    private void Start()
    {
        if (damageable != null)
        {
            damageable.OnDeath += HandleDeath;
        }
    }

    private void HandleDeath()
    {
        CurrencyManager.Instance.AddGold(goldReward);
    }

    private void OnDestroy()
    {
        if (damageable != null)
        {
            damageable.OnDeath -= HandleDeath;
        }
    }
}
