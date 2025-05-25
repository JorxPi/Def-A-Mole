using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    public Director director;
    public Transform[] spawnPoints;
    public float spawnInterval = 0.5f;

    private List<GameObject> currentWave; // Cached wave

    public void StartLevel(int level)
    {
        currentWave = director.GenerateEnemyWave(level);
        StartCoroutine(SpawnWaveCoroutine(currentWave));
    }

    private IEnumerator SpawnWaveCoroutine(List<GameObject> wave)
    {
        foreach (GameObject enemyPrefab in wave)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            int hpBonus = LevelManager.Instance.GetEnemyHpBonus();
            int dmgBonus = LevelManager.Instance.GetEnemyDamageBonus();

            if (enemyGO.TryGetComponent<Damageable>(out var dmg))
                dmg.IncreaseMaxHealth(hpBonus);

            if (enemyGO.TryGetComponent<EnemyAI>(out var ai))
                ai.IncreaseDamage(dmgBonus);

            EnemyAI enemyAI = enemyGO.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                LevelManager.Instance.AssignTargetToEnemy(enemyAI);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public int GetEnemiesToSpawn(int level)
    {
        // Return size of cached wave if available and same level
        if (currentWave != null)
            return currentWave.Count;

        // Fallback if StartLevel hasn't been called yet
        List<GameObject> tempWave = director.GenerateEnemyWave(level);
        return tempWave.Count;
    }
}

