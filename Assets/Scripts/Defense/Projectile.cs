using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Transform target;
    protected float damage;

    public void SetTarget(Transform Target)
    {
        target = Target;
    }

    public void SetDamage(float Damage)
    {
        damage = Damage;
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
        if (target.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
