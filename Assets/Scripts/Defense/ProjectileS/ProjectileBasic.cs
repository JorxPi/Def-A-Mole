using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBasic : Projectile
{
    public float speed = 10f;

    protected override void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction * distanceThisFrame, Space.World);
    }
}
