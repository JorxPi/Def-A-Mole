using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBrainBasic : DBrain
{
    public GameObject projectilePrefab;
    //position from where the projectile is fired (every defense will have a gamobject chils with a firePoint transform)
    public Transform firePoint;

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

    protected override void PerformAttack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileInstance.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetTarget(target);
            projectile.SetDamage(defense.attackDamage);
        }
    }
}
