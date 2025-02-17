using UnityEngine;

public class SplitEnemy : Enemy
{
    public GameObject smallerEnemyPrefab;
    public int splitCount = 2;
    public float spawnRadius = 0.5f; // Radio para el spawn de los enemigos pequeños
    
    protected override void HandleDeath()
    {
        // Obtener la posición objetivo del enemigo actual
        Vector3 currentPosition = transform.position;
        
        // Crear enemigos más pequeños
        for (int i = 0; i < splitCount; i++)
        {
            // Calcular una posición offset aleatoria en el plano XZ
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 offset = new Vector3(randomCircle.x, 0, randomCircle.y);
            
            // Instanciar el enemigo pequeño
            GameObject smallerEnemy = Instantiate(smallerEnemyPrefab, currentPosition + offset, transform.rotation);
            
            // Obtener y configurar el EnemyAI del enemigo pequeño
            EnemyAI enemyAI = smallerEnemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                // Asignar el mismo punto objetivo que tenía el enemigo original
                enemyAI.SetTarget(transform.position + transform.forward * 10f);
            }
        }
        
        // Llamar al HandleDeath base después de crear los enemigos pequeños
        base.HandleDeath();
    }
}