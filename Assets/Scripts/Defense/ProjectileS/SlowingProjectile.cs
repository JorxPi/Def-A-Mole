// --- SlowingProjectile.cs ---

using UnityEngine;

public class SlowingProjectile : Projectile
{
    public float speed = 10f;
    public float height = 5f;

    public float explosionRadius { get; set; } = 3f;
    public float slowAmount = 50f;    // percent
    public float slowDuration = 2f;

    private Vector3 startPoint, targetPoint;
    private float travelTime, elapsedTime;

    protected override void Start()
    {
        base.Start();
        startPoint = transform.position;
        elapsedTime = 0f;

        // Predict targetPoint as before...
        if (target != null && target.TryGetComponent<Enemy>(out var e))
        {
            float eta = Vector3.Distance(startPoint, target.position) / speed;
            Vector3 pred = target.position + (e.enemyAI.GetVelocity() * eta);
            pred.y = target.position.y;
            targetPoint = pred;
        }
        else
        {
            targetPoint = target.position;
        }

        travelTime = Vector3.Distance(startPoint, targetPoint) / speed;
    }

    protected override void MoveTowardsTarget()
    {
        elapsedTime += Time.deltaTime;
        float p = Mathf.Clamp01(elapsedTime / travelTime);

        Vector3 horiz = Vector3.Lerp(startPoint, targetPoint, p);
        float arcY = 4f * height * p * (1 - p);
        transform.position = new Vector3(horiz.x, horiz.y + arcY, horiz.z);

        if (p >= 1f) HitTarget();
    }

    protected override void HitTarget()
    {
        // 1) VFX if any
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);

        // 2) Splash & slow
        var hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                // apply the cached damageValue
                if (col.TryGetComponent<Damageable>(out var dmg))
                    dmg.ApplyDamage(damageValue, attacker);

                // apply slow
                if (col.TryGetComponent<Enemy>(out var enemy))
                    enemy.enemyAI.ApplySlow(slowAmount, slowDuration);
            }
        }

        // 3) destroy self
        Destroy(gameObject);
    }
}
