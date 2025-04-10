using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : Enemy
{
    public GameObject smallerEnemyPrefab;
    public int splitCount = 2;
    public float spawnRadius = 0.5f; // Radio para el spawn de los enemigos pequeños

    protected override void HandleDeath()
    {
        SpawnChildren();

        // Now call base Die manually
        Damageable damageable = GetComponent<Damageable>();
        damageable.OnDeath -= HandleDeath; // Avoid double calls
        StartCoroutine(DieAfterSplit());
    }

    private void SpawnChildren()
    {
        Vector3 currentPosition = transform.position;
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        for (int i = 0; i < splitCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 offset = new Vector3(randomCircle.x, 0, randomCircle.y);

            GameObject smallerEnemy = Instantiate(smallerEnemyPrefab, currentPosition + offset, transform.rotation);

            if (smallerEnemy.TryGetComponent<EnemyAI>(out EnemyAI ai) && GetComponent<EnemyAI>() is EnemyAI thisAI)
            {
                ai.SetTarget(thisAI.GetBarrierPoint());
            }

            levelManager.enemiesAlive++;
        }
    }

    private IEnumerator DieAfterSplit()
    {
        yield return null; // Wait one frame

        // Proceed with normal death behavior
        base.HandleDeath();
    }

    private bool IsInsideWarningZone(GameObject enemy)
    {
        GameObject warningZoneObj = GameObject.FindGameObjectWithTag("WarningZone");
        if (warningZoneObj == null)
        {
            Debug.LogWarning("WarningZone no encontrada en la escena.");
            return false;
        }

        Collider warningCollider = warningZoneObj.GetComponent<Collider>();
        if (warningCollider == null)
        {
            Debug.LogWarning("El objeto WarningZone no tiene un Collider.");
            return false;
        }

        return !warningCollider.bounds.Contains(enemy.transform.position);
    }
}
