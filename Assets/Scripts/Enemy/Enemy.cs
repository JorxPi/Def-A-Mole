using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50f;
    public int goldReward = 20;

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        //CurrencyManager.Instance.AddGold(goldReward);
        Destroy(gameObject);
    }
}
