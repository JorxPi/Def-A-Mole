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

        if (Input.GetMouseButtonDown(1))
        {
            // Quitamos highlight de la actual
            Highlightable.DeselectCurrent();
            // Ocultamos los paneles (igual que haces hoy)
            UpgradeUI.Instance.HideUpgradePanel();
            SellUI.Instance.HideSellPanel();
        }

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
        Transform baseTransform = transform.Find("Base");
        if (baseTransform == null)
        {
            Debug.LogError($"Base not found under {gameObject.name}!");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int mask = LayerMask.GetMask("BaseLayer");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask) &&
                hit.transform == baseTransform)
            {
                // 1) Resaltamos esta defensa
                Highlightable.DeselectCurrent();
                var high = GetComponent<Highlightable>();
                if (high != null)
                    high.Select();

                // 2) Mostramos tus UIs
                UpgradeUI.Instance.ShowUpgradePanel(gameObject);
                SellUI.Instance.ShowSellPanel(gameObject);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        OnDefenseDestroyed?.Invoke(this);
    }
}
