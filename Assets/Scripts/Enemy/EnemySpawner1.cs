using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner1 : MonoBehaviour
{
    [Header("Enemy Configuration")]
    public GameObject[] enemyPrefabs; // Prefabs de enemigos disponibles
    public Transform spawnPoint; // Punto de spawn
    public float spawnInterval = 1f; // Tiempo entre spawns
    public int maxEnemiesPerLevel = 50; // Límite máximo de enemigos por nivel

    [Header("Map Configuration")]
    public float mapRadius = 10f; // Radio del mapa

    private int currentLevel = 1; // Nivel actual
    private int enemiesToSpawn; // Total de enemigos por nivel
    private int enemiesSpawned; // Contador de enemigos spawneados
    private List<GameObject> activeEnemies = new List<GameObject>(); // Lista de enemigos vivos

    private void Start()
    {
        StartLevel(currentLevel); // Comienza el primer nivel
    }

    private void StartLevel(int level)
    {
        Debug.Log($"Starting Level {level}");

        // Calcula cuántos enemigos spawnear en este nivel
        enemiesToSpawn = CalculateEnemiesForLevel(level);
        enemiesSpawned = 0;

        // Limpia la lista de enemigos activos (por si acaso)
        activeEnemies.Clear();

        // Inicia el spawn de enemigos
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < enemiesToSpawn)
        {
            // Selecciona un prefab de enemigo al azar
            GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Calcula el punto objetivo para el enemigo
            Vector3 targetPoint = CalculateTargetPoint();

            // Instancia al enemigo
            GameObject enemy = Instantiate(selectedEnemyPrefab, spawnPoint.position, Quaternion.identity);

            // Asigna el punto objetivo al enemigo
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemy.GetComponent<EnemyAI>() != null)
            {
                enemy.GetComponent<EnemyAI>().SetTarget(targetPoint);
            }

            // Agrega el enemigo a la lista de activos
            activeEnemies.Add(enemy);

            enemiesSpawned++;

            // Espera antes de spawnear el siguiente enemigo
            yield return new WaitForSeconds(spawnInterval);
        }

        // Espera hasta que todos los enemigos sean eliminados
        yield return new WaitUntil(() => activeEnemies.Count == 0);

        // Cuando se matan todos los enemigos, avanza al siguiente nivel
        currentLevel++;
        StartLevel(currentLevel);
    }

    private int CalculateEnemiesForLevel(int level)
    {
        // Fórmula para calcular enemigos por nivel
        if (level <= 10)
        {
            return Mathf.RoundToInt(5 + Mathf.Pow(level, 1.5f)); // Aumento rápido en niveles bajos
        }
        else if (level <= 20)
        {
            return Mathf.RoundToInt(30 + (level - 10) * 2); // Aumento más lento
        }
        else
        {
            return Mathf.Clamp(50 + (level - 20), 50, maxEnemiesPerLevel); // Estabilización
        }
    }

    private Vector3 CalculateTargetPoint()
    {
        // Genera un ángulo aleatorio dentro del rango permitido
        float randomAngle = Random.Range(0, 360);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        // Calcula la posición en el borde del mapa
        float x = Mathf.Cos(angleInRadians) * mapRadius;
        float z = Mathf.Sin(angleInRadians) * mapRadius;

        return new Vector3(x, 0, z);
    }

    public void NotifyEnemyKilled(GameObject enemy)
    {
        // Remueve al enemigo de la lista de activos
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}
