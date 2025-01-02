using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all defenses
public class Defense : MonoBehaviour
{
    [Header("Base Stats")]
    public DefenseData baseStats;

    public float attackRange;
    public float attackDamage;
    public float attackSpeed;

    protected float attackCooldown = 0f;

    protected virtual void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        if (baseStats != null)
        {
            attackRange = baseStats.attackRange;
            attackDamage = baseStats.attackDamage;
            attackSpeed = baseStats.attackSpeed;
        }
    }

    protected virtual void Update()
    {
        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
    }

    public bool CanAttack()
    {
        return attackCooldown <= 0f;
    }

    public void ResetAttackCooldown()
    {
        attackCooldown = 1f / attackSpeed;
    }

    private void OnMouseDown()
    {
        UpgradeUI.Instance.ShowUpgradePanel(gameObject);
    }
}
