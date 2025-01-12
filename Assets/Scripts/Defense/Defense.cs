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

    [Header("Layer Settings")]
    public LayerMask defenseLayer;

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

        DetectDefenseClick();
    }

    public bool CanAttack()
    {
        return attackCooldown <= 0f;
    }

    public void ResetAttackCooldown()
    {
        attackCooldown = 1f / attackSpeed;
    }

    private void DetectDefenseClick()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, defenseLayer.value))
            {
                if (hit.transform == transform) 
                {
                    Debug.Log($"Clicked on Defense: {gameObject.name}");
                    UpgradeUI.Instance.ShowUpgradePanel(gameObject);
                }
            }
        }
    }
}
