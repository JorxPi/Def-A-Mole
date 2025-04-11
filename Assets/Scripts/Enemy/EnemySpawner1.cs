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

        int hpBonus = levelManager.GetEnemyHpBonus();
        int damageBonus = levelManager.GetEnemyDamageBonus();

        Damageable dmg = enemy.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.health += hpBonus;
            dmg.maxHealth = dmg.health;
        }

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.damageAmount += damageBonus;
        }

        levelManager.AssignTargetToEnemy(enemy.GetComponent<EnemyAI>());
    }

    private int CalculateEnemiesForLevel(int level)
    {
        if (level <= 10)
            return Mathf.RoundToInt(2 + Mathf.Pow(level, 1.5f));
        if (level <= 20)
            return Mathf.RoundToInt(30 + (level - 10) * 2);
        return Mathf.Clamp(50 + (level - 20), 50, 100);
    }

    public int GetEnemiesToSpawn(int level)
    {
        return CalculateEnemiesForLevel(level);
    }
}
