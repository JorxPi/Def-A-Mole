using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all defenses
public class Defense : MonoBehaviour
{

    [Header("Priority")]
    public int priority = 1;

    [Header("Base Stats")]
    public DefenseData baseStats;
    public float attackRange;
    public float attackDamage;
    public float attackSpeed;
    public float attractionRange;

    [Header("Layer Settings")]
    public LayerMask defenseLayer;

    protected float attackCooldown = 0f;

    public static event Action<Defense> OnDefenseSpawned;
    public static event Action<Defense> OnDefenseDestroyed;

    protected virtual void Start()
    {
        InitializeStats();
        OnDefenseSpawned?.Invoke(this);
    }

    private void InitializeStats()
    {
        if (baseStats != null)
        {
            attackRange = baseStats.attackRange;
            attackDamage = baseStats.attackDamage;
            attackSpeed = baseStats.attackSpeed;
            attractionRange = GetComponent<SphereCollider>().radius;
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
        Transform baseTransform = transform.Find("Base"); // Find Base automatically

        if (baseTransform == null)
        {
            Debug.LogError($"Base not found under {gameObject.name}!");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BaseLayer")))
            {
                if (hit.transform == baseTransform) 
                {
                    UpgradeUI.Instance.ShowUpgradePanel(gameObject);

                    SellUI.Instance.ShowSellPanel(gameObject);
                }
            }
        }
    }

    protected virtual void OnDestroy()
    {
        OnDefenseDestroyed?.Invoke(this);
    }
}
