using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 targetPoint;
    private bool hasTarget = false;

    private void Update()
    {
        if (hasTarget)
        {
            MoveTowardsTarget();
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPoint = target;
        hasTarget = true;
    }

    private void MoveTowardsTarget()
    {
        // Movimiento hacia el objetivo
        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Comprueba si llegó al objetivo
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            hasTarget = false;
            OnReachTarget();
        }
    }

    private void OnReachTarget()
    {
        // Lógica cuando el enemigo alcanza el objetivo
        Debug.Log("Enemy reached the target!");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Notifica al LevelManager o Spawner si es necesario
    }
}
