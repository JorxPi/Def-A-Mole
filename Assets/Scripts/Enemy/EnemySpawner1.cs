using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    public Director director;
    public Transform[] spawnPoints;

    public void StartLevel(int level)
    {
        List<GameObject> wave = director.GenerateEnemyWave(level);

        foreach (GameObject enemyPrefab in wave)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public int GetEnemiesToSpawn(int level)
    {
        return director.GenerateEnemyWave(level).Count;
    }
}

