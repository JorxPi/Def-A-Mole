using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f; // Velocidad a la que se mueve el enemigo
    private Vector3 targetPoint; // Punto objetivo hacia el que se moverá
    private EnemyAnimation enemyAnimation; // Animaciones del enemigo
    private bool hasTarget = false; // Controla si el enemigo tiene un objetivo asignado

    private void Start()
    {
        // Obtén el componente de animación
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    public void SetTarget(Vector3 target)
    {
        targetPoint = target;
        hasTarget = true;

        // Activar la animación de caminar
        if (enemyAnimation != null)
        {
            enemyAnimation.SetWalking(true);
        }
    }

    private void Update()
    {
        if (hasTarget)
        {
            // Calcula la dirección hacia el punto objetivo
            Vector3 direction = (targetPoint - transform.position).normalized;

            // Mueve al enemigo hacia el objetivo
            transform.position += direction * speed * Time.deltaTime;

            // Verifica si el enemigo ha llegado al destino
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                hasTarget = false; // Detiene el movimiento
                if (enemyAnimation != null)
                {
                    enemyAnimation.SetWalking(false); // Detener animación de caminar
                }
                OnReachTarget(); // Lógica al llegar al destino
            }
        }
    }

    private void OnReachTarget()
    {
        // Lógica que quieres ejecutar cuando el enemigo llega al destino
        Debug.Log("Enemy reached the target!");
        // Ejemplo: Aplica daño a la barrera o destruye al enemigo
    }
}