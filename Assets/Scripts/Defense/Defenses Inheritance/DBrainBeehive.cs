using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBrainBeehive : DBrain
{
    private float damageCooldown = 0f; 

    protected override void Update()
    {
        if (damageCooldown <= 0f && defense.CanAttack())
        {
            DealAreaDamage();
            defense.ResetAttackCooldown();
            damageCooldown = 1f / defense.attackSpeed;
        }

        if (damageCooldown > 0f)
        {
            damageCooldown -= Time.deltaTime;
        }
    }

    protected override void ChooseTarget()
    {
        target = null;
    }

    private void DealAreaDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, defense.attackRange);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                ApplyDamage(collider);
            }
        }
    }

    private void ApplyDamage(Collider enemy)
    {
        if (enemy.TryGetComponent<Damageable>(out Damageable damageable))
        {
            damageable.ApplyDamage(defense.attackDamage);
        }
    }

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, defense.attackRange);
    }*/
}
