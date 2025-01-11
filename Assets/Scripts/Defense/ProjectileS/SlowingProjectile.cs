using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingProjectile : Projectile
{
    public float speed = 10f;
    public float height = 5f;

    public float explosionRadius { get; set; } = 3f; // Upgradeable
    public float slowAmount { get; set; } = 0.5f;    // Upgradeable
    public float slowDuration = 2f;

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
                    //enemy.ApplySlow(slowAmount, slowDuration);
                }
            }
        }

        Destroy(gameObject); 
    }
}
