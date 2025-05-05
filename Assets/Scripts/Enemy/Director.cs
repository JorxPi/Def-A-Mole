using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBudgetEntry
{
    public GameObject enemyPrefab;
    public int cost;
    public float weight = 1f; // optional: helps add variety
}

public class Director : MonoBehaviour
{
    public List<EnemyBudgetEntry> enemyTypes;

    public int baseSalary = 10;
    public int salaryIncreasePerLevel = 5;

    public List<GameObject> GenerateEnemyWave(int level)
    {
        if (level == 1)
        {
            // Always spawn two TopoWalks on Round 1
            return new List<GameObject>
            {
                enemyTypes[0].enemyPrefab,
                enemyTypes[0].enemyPrefab
            };
        }

        List<GameObject> wave = new List<GameObject>();

        int currentSalary = baseSalary + (salaryIncreasePerLevel * (level - 1));
        int budget = currentSalary;

        while (budget > 0)
        {
            List<EnemyBudgetEntry> affordable = enemyTypes.FindAll(e => e.cost <= budget);
            if (affordable.Count == 0) break;

            EnemyBudgetEntry selected = GetWeightedRandom(affordable);
            wave.Add(selected.enemyPrefab);
            budget -= selected.cost;
        }

        return wave;
    }

    private EnemyBudgetEntry GetWeightedRandom(List<EnemyBudgetEntry> options)
    {
        float totalWeight = 0f;
        foreach (var e in options)
            totalWeight += e.weight;

        float rand = Random.Range(0f, totalWeight);
        float running = 0f;

        foreach (var e in options)
        {
            running += e.weight;
            if (rand <= running)
                return e;
        }

        return options[0];
    }
}
