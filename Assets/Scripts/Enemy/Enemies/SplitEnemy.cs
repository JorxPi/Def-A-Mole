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
        // Obtener la posición objetivo del enemigo actual
        Vector3 currentPosition = transform.position;
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        DamageVignetteController warningZone = FindObjectOfType<DamageVignetteController>();

        for (int i = 0; i < splitCount; i++)
        {
            // Calcular una posición offset aleatoria en el plano XZ
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 offset = new Vector3(randomCircle.x, 0, randomCircle.y);
            
            // Instanciar el enemigo pequeño
            GameObject smallerEnemy = Instantiate(smallerEnemyPrefab, currentPosition + offset, transform.rotation);
            
            // Si el enemigo pequeño tiene un EnemyAI, establecer su objetivo
            if (smallerEnemy.TryGetComponent<EnemyAI>(out EnemyAI ai) && this.TryGetComponent<EnemyAI>(out EnemyAI thisAI))
            {
                ai.SetTarget(thisAI.GetBarrierPoint());
            }
            levelManager.enemiesAlive++;

            if (IsInsideWarningZone(smallerEnemy))
            {
                ai.isInBarrier = true;
            }

        }

        // Llamar al HandleDeath base después de crear los enemigos pequeños
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
