using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DBrain : MonoBehaviour
{
    protected Defense defense;
    protected Transform target;

    public Transform Target => target; 
    public Defense Defense => defense;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    protected virtual void Awake()
    {
        defense = GetComponent<Defense>();
    }

    protected virtual void Update()
    {
        ChooseTarget();
        if (target != null && defense.CanAttack())
        {
            PerformAttack();
            defense.ResetAttackCooldown();
        }
    }

    protected abstract void ChooseTarget();

    protected virtual void PerformAttack()
    {
        if (projectilePrefab == null || firePoint == null || target == null) return;

        GameObject projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.Initialize(target, defense);
    }
}