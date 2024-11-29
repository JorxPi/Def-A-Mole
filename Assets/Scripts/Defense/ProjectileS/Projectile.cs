using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Transform target;
    protected MonoBehaviour owner;

    public void Initialize(Transform Target, MonoBehaviour Owner)
    {
        target = Target;
        owner = Owner; 
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();
    }

    protected abstract void MoveTowardsTarget();

    //this should be in the enemy. They should check if a projectile enter the trigger zone and recive damage acordingly.
    //leaving this here for testing
    protected virtual void HitTarget()
    {
        if (target.TryGetComponent<Damageable>(out Damageable damageable))
        {
            if (owner is Defense defense)
            {
                float damage = defense.attackDamage;
                damageable.ApplyDamage(damage);
            }
        }

        Destroy(gameObject); // Destroy projectile after hitting target
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if projectile hitsthe target
        if (other.transform == target)
        {
            HitTarget();
        }
    }
}
