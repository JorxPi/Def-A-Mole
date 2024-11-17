using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBrainSniper : DBrain
{
    protected override void ChooseTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, defense.attackRange);
        float lowestHP = Mathf.Infinity;
        Transform weakestEnemy = null;

        foreach (var enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                Enemy enemy = enemyCollider.GetComponent<Enemy>();
                if (enemy != null && enemy.health < lowestHP)
                {
                    lowestHP = enemy.health;
                    weakestEnemy = enemy.transform;
                }
            }
        }

        target = weakestEnemy;
    }
}