using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // Velocidad a la que se mueve el enemigo
    private Vector3 moveDirection;
    private EnemyAnimation enemyAnimation;

    private void Start()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;

        // Activar la animación de caminar
        enemyAnimation.SetWalking(true);
    }

    private void Update()
    {
        // Mover al enemigo en la dirección especificada
        transform.position += moveDirection * speed * Time.deltaTime;

        // Si el enemigo no se está moviendo, detener la animación
        if (moveDirection == Vector3.zero)
        {
            enemyAnimation.SetWalking(false);
        }
    }
}
