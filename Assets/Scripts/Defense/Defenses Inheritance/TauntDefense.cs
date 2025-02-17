using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntDefense : Defense
{
    [Header("Taunt Defense Settings")]
    public float retaliatoryDamage = 10f;

    protected override void Start()
    {
        base.Start();

    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Enemy"))
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null && enemy.enemyAI != null)
        {
            if (other is SphereCollider)
            {                
                enemy.enemyAI.SetTarget(this.transform.position); // Updated method
            }

            if (other is BoxCollider)
            {
                enemy.enemyAI.AttackDefenseTarget(this.GetComponent<Defense>()); // Updated method
            }
        }
    }
}


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