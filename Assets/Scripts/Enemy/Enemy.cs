using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Damageable damageable;
    public int goldReward = 20;

    /*public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0f)
        {
            Die();
        }
    }*/

    /*public void ApplyDamage(MonoBehaviour attacker)
    {
        Debug.Log(attacker);
        if (attacker is Defense defense)
        {
            Debug.Log(attacker);
            float damage = defense.attackDamage; 
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        //CurrencyManager.Instance.AddGold(goldReward);
        Destroy(gameObject);
    }*/
}
