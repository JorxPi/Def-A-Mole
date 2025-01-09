using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    public float explosionRadius = 3f;
    public float explosionDamage = 50f;
    
    protected override void HandleDeath()
    {
        // Crear explosión
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageable.ApplyDamage(explosionDamage);
            }
        }
        
        base.HandleDeath();
    }
}