using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public float damageAmount = 10f;
    public float stopDistance = 0.5f; // Distancia al borde para detenerse
    private Vector3 targetPoint;
    private bool hasTarget = false;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        agent.destination = targetPoint;

        /*
        if (hasTarget)
        {
            // Establecer destino del agente
            agent.destination = targetPoint;

            // Comprobar si está cerca del objetivo
            if (Vector3.Distance(transform.position, targetPoint) <= stopDistance)
            {
                StopAtEdge();
            }
        }
*/
    }

    public void SetTarget(Vector3 target)
    {
        targetPoint = target;
        hasTarget = true;
    }

    private void StopAtEdge()
    {
        agent.isStopped = true; // Detiene el movimiento del agente
        hasTarget = false; // Detener futuras actualizaciones de movimiento
    }
}