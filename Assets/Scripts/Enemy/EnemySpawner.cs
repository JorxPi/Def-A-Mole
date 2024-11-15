using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo que vas a instanciar
    public Transform spawnCenter; // Centro desde donde se generarán los enemigos
    public float spawnRadius = 1f; // Radio del círculo desde donde se generarán los enemigos
    public float spawnHeight = 0f; // Altura adicional a la que se generarán los enemigos
    public int numberOfEnemies = 10; // Número de enemigos a generar
    public float spawnInterval = 2f; // Intervalo entre cada generación de enemigos

    private void Start()
    {
        // Comienza a generar enemigos periódicamente
        InvokeRepeating("SpawnEnemies", 0f, spawnInterval);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Asegurarse de que hay un centro de spawn definido
            if (spawnCenter == null)
            {
                Debug.LogError("No se ha asignado un centro de spawn.");
                return;
            }

            // Generar un ángulo aleatorio
            float angle = Random.Range(0f, 360f);
            Vector3 spawnPosition = spawnCenter.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;

            // Ajustar la posición de spawn en la altura
            spawnPosition.y += spawnHeight - 1;

            // Calcular la dirección hacia afuera desde el centro
            Vector3 direction = (spawnPosition - spawnCenter.position).normalized;
            direction.y = 0; // Mantener el movimiento horizontal para que no se desplace hacia arriba/abajo

            // Instanciar el enemigo en la posición generada y orientarlo hacia afuera
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.LookRotation(direction));

            // Hacer que el enemigo se mueva hacia afuera
            EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.SetDirection(direction);
            }
        }
    }
}
