using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float damageAmount = 10f;
    private Vector3 targetPoint;
    private bool hasTarget = false;
    private bool hasReachedTarget = false;

    private void Update()
    {
        if (hasTarget && !hasReachedTarget)
        {
            MoveTowardsTarget();
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPoint = target;
        hasTarget = true;
        hasReachedTarget = false;
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            hasReachedTarget = true;
            OnReachTarget();
        }
    }

    private void OnReachTarget()
    {
        var baseHealth = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<Damageable>();
        if (baseHealth != null)
        {
            baseHealth.ApplyDamage(damageAmount);
        }
    }
}