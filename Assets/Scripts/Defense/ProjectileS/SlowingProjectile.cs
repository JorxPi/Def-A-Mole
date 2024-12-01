using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingProjectile : Projectile
{
    public float speed = 10f;
    public float slowDuration = 2f;
    public float slowAmount = 0.5f; // Slow enemies to 50% of their original speed
    public float explosionRadius = 3f;

    private Vector3 launchPosition;
    private float height = 5f; // Peak height of the parabolic arc

    protected override void Start()
    {
        base.Start();
        launchPosition = transform.position;
    }

    protected override void MoveTowardsTarget()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate progress along the trajectory
        float distanceToTarget = Vector3.Distance(launchPosition, target.position);
        float progress = Mathf.Clamp01(Vector3.Distance(transform.position, launchPosition) / distanceToTarget);

        // Calculate parabolic height
        float yOffset = 4 * height * (progress - progress * progress); // Simple parabolic formula

        // Move towards the target along the arc
        Vector3 direction = (target.position - launchPosition).normalized;
        Vector3 nextPosition = Vector3.Lerp(launchPosition, target.position, progress);
        nextPosition.y += yOffset;

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

        // Check if we've reached the target
        if (progress >= 1f)
        {
            HitTarget();
        }
    }

    protected override void HitTarget()
    {
        // Area effect: Apply damage and slow to all enemies within radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Apply damage
                if (collider.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (owner is Defense defense)
                    {
                        float damage = defense.attackDamage;
                        damageable.ApplyDamage(damage);
                    }
                }

                // Apply slow (Assume enemies have a method `ApplySlow`)
                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    //enemy.ApplySlow(slowAmount, slowDuration);
                }
            }
        }

        Destroy(gameObject); // Destroy the projectile
    }
}
