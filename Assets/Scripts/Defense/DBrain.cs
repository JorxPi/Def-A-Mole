using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DBrain : MonoBehaviour
{
    protected Defense defense;
    protected Transform target;

    public Transform Target => target; 
    public Defense Defense => defense;

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

    protected abstract void PerformAttack();
}