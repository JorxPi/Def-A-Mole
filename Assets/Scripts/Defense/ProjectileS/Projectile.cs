using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Transform target;
    protected MonoBehaviour owner;

    public GameObject hitEffectPrefab;

    protected virtual void Start()
    {
        // Base initialization
    }

    public void Initialize(Transform Target, MonoBehaviour Owner)
    {
        target = Target;
        owner = Owner;
        AudioManager.Instance.PlayDefenseShoot(1f);
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

    protected virtual void HitTarget()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        if (target.TryGetComponent<Damageable>(out Damageable damageable))
        {
            if (owner is Defense defense)
            {
                float damage = defense.attackDamage;
                damageable.ApplyDamage(damage, defense.gameObject);
            }
        }

        Destroy(gameObject); // Destroy projectile after hitting target
    }

    // can do this with pure math
    private void OnTriggerEnter(Collider other)
    {
        // Check if projectile hitsthe target
        if (other.transform == target)
        {
            HitTarget();
        }
    }
}
