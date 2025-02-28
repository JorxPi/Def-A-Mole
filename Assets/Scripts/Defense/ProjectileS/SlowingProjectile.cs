using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingProjectile : Projectile
{
    public float speed = 10f;
    public float height = 5f;

    public float explosionRadius { get; set; } = 3f; // Upgradeable
    public float slowAmount { get; set; } = 50f;    // Upgradeable - Percentage
    public float slowDuration = 2f;

    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float travelTime;
    private float elapsedTime;

    protected override void Start()
    {
        base.Start();
        startPoint = transform.position;

        if (target != null && target.TryGetComponent<Enemy>(out Enemy enemy))
        {
            float estimatedTravelTime = Vector3.Distance(startPoint, target.position) / speed;
            Vector3 predictedPosition = target.position + (enemy.enemyAI.GetVelocity() * estimatedTravelTime);

            predictedPosition.y = target.position.y;

            targetPoint = predictedPosition;
        }
        else
        {
            targetPoint = target.position;
        }

        travelTime = Vector3.Distance(startPoint, targetPoint) / speed;
        elapsedTime = 0f;
    }

    protected override void MoveTowardsTarget()
    {
        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / travelTime);

        Vector3 horizontalPosition = Vector3.Lerp(startPoint, targetPoint, progress);

        float parabolicHeight = 4 * height * progress * (1 - progress);

        Vector3 nextPosition = new Vector3(horizontalPosition.x, horizontalPosition.y + parabolicHeight, horizontalPosition.z);
        transform.position = nextPosition;

        if (progress >= 1f)
        {
            HitTarget();
        }
    }


    protected override void HitTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                if (collider.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    if (owner is Defense defense)
                    {
                        float damage = defense.attackDamage;
                        damageable.ApplyDamage(damage);
                    }
                }
                
                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.enemyAI.ApplySlow(slowAmount,slowDuration);
                }
            }
        }

        Destroy(gameObject); 
    }
}
