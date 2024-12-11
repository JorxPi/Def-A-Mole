using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingProjectile : Projectile
{
    public float speed = 10f;
    public float height = 5f;

    public float explosionRadius = 3f;
    public float slowDuration = 2f;
    public float slowAmount = 0.5f;

    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float travelTime;
    private float elapsedTime;

    protected override void Start()
    {
        base.Start();
        startPoint = transform.position;
        targetPoint = target.position;

        travelTime = Vector3.Distance(startPoint, targetPoint) / speed;
        elapsedTime = 0f;
    }

    protected override void MoveTowardsTarget()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / travelTime);

        // Calculate the horizontal position
        Vector3 horizontalPosition = Vector3.Lerp(startPoint, targetPoint, progress);

        // Calculate the parabolic height offset
        float parabolicHeight = 4 * height * progress * (1 - progress);

        // Set the new position with the height offset
        Vector3 nextPosition = new Vector3(horizontalPosition.x, horizontalPosition.y + parabolicHeight, horizontalPosition.z);
        transform.position = nextPosition;

        // Check if the projectile has reached the target
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

                // Apply slow effect
                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    //enemy.ApplySlow(slowAmount, slowDuration);
                }
            }
        }

        Destroy(gameObject); // Destroy the projectile after applying effects
    }
}
