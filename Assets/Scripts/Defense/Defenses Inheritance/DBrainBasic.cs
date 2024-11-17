using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBrainBasic : DBrain
{
    protected override void ChooseTarget()
    {
        // Example logic: Find nearest enemy within range
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, defense.attackRange);
        float closestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (var enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }
        }

        target = nearestEnemy;
    }
}
