using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntDefense : Defense
{
    [Header("Taunt Defense Settings")]
    public float retaliatoryDamage = 10f;

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (other is SphereCollider)
            {                
                enemy.enemyAI.SetTowerTarget(this.transform.position);
               
            }

            if (other is BoxCollider)
            {
                enemy.enemyAI.AttackTower(this.GetComponent<Defense>());
            }

        }
    }
    */

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