using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntDefense : Defense
{
    [Header("Taunt Defense Settings")]
    public float retaliatoryDamage = 10f;
    private SphereCollider tauntRangeCollider;

    protected override void Start()
    {
        base.Start();

        tauntRangeCollider = gameObject.AddComponent<SphereCollider>();
        tauntRangeCollider.isTrigger = true;
        tauntRangeCollider.radius = attackRange;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetPriorityTarget(this.transform);
            }
        }
    }*/

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ResetTargetPriority();
            }
        }
    }*/
}