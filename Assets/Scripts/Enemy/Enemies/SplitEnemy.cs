using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : Enemy
{
    public GameObject smallerEnemyPrefab;
    public int splitCount = 2;
    
    protected override void HandleDeath()
    {
        // Crear enemigos más pequeños
        for (int i = 0; i < splitCount; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 0.5f;
            offset.y = 0;
            GameObject smallerEnemy = Instantiate(smallerEnemyPrefab, transform.position + offset, transform.rotation);
            
            // Si el enemigo pequeño tiene un EnemyAI, establecer su objetivo
            if (smallerEnemy.TryGetComponent<EnemyAI>(out EnemyAI ai))
            {
                //ai.SetTarget(enemyAI.GetTargetPosition());
            }
        }
        
        base.HandleDeath();
    }
}