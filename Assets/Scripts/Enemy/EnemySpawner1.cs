using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class EnemySpawner1 : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Tipos de enemigos disponibles
    public Transform spawnPoint; // Punto de spawn
    public float spawnInterval = 2f; // Tiempo entre spawns

    private int enemiesToSpawn;
    private int enemiesSpawned;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        StartLevel(1); // Comienza en el nivel 1
    }

    public void StartLevel(int level)
    {
        enemiesToSpawn = CalculateEnemiesForLevel(level);
        enemiesSpawned = 0;

        // Comienza a spawnear enemigos
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < enemiesToSpawn)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);

        levelManager.AssignTargetToEnemy(enemy.GetComponent<EnemyAI>());

        levelManager.enemiesAlive++;
        Debug.Log($"Enemigo ha sido creado. ({levelManager.enemiesAlive} enemigos)");
    }

    private int CalculateEnemiesForLevel(int level)
    {
        if (level <= 10)
            return Mathf.RoundToInt(2 + Mathf.Pow(level, 1.5f));
        if (level <= 20)
            return Mathf.RoundToInt(30 + (level - 10) * 2);
        return Mathf.Clamp(50 + (level - 20), 50, 100);
    }
}
