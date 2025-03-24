using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
    public float damageAmount = 10f;
    public float attackInterval = 1f;
    public float baseSpeed = 3.5f;
    public float attackRange = 2f;
    public float aggroRange = 3f;

    private Vector3 barrierPoint;
    private bool isAttackingDefense = false;
    [HideInInspector]
    public bool isAttackingBarrier = false, isInBarrier = false;
    private bool canAttack = false;

    private bool isSlowed = false;
    private float slowTimer = 0f;
    
    // Añadir una variable para controlar si el enemigo está muriendo
    [HideInInspector]
    public bool isDying = false;

    private Vector3 defensePoint;

    private NavMeshAgent agent;
    private Barrier targetBarrier;
    private Defense targetDefense;
    private float attackTimer;
    private DamageVignetteController vignetteController;

    void OnEnable()
    {
        Defense.OnDefenseSpawned += HandleDefenseSpawned;
        Defense.OnDefenseDestroyed += HandleDefenseDestroyed;
    }

    void OnDisable()
    {
        Defense.OnDefenseSpawned -= HandleDefenseSpawned;
        Defense.OnDefenseDestroyed -= HandleDefenseDestroyed;
    }

    void Start()
    {
        Debug.Log($"START ENEMY");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;

        // Solo establecer el destino si el punto no es cero
        if (barrierPoint != Vector3.zero)
        {
            agent.SetDestination(barrierPoint);
        }

        GameObject barrierObject = GameObject.FindWithTag("Barrier");
        if (barrierObject != null)
        {
            targetBarrier = barrierObject.GetComponent<Barrier>();
        }

        vignetteController = FindObjectOfType<DamageVignetteController>();

        Defense defenseInRange = FindNearestDefense();
        if (defenseInRange != null)
        {
            targetDefense = defenseInRange;
            isAttackingDefense = true;
            agent.SetDestination(defenseInRange.transform.position);
        }
    }

    private void Update()
    {
        // No actualizar la lógica si el enemigo está muriendo o si el agente no está activo
        if (isDying || agent == null || !agent.isOnNavMesh || !agent.enabled)
        {
            return;
        }
        
        if (isAttackingDefense && targetDefense != null)
        {
            float distanceToDefense = Vector3.Distance(transform.position, targetDefense.transform.position);
            if (distanceToDefense <= attackRange + 3)
            {
                if (!canAttack)
                {
                    canAttack = true;
                }
            }
            else
            {
                if (canAttack)
                {
                    canAttack = false;
                }
            }
        }
        else
        {
            // Verificar que barrierPoint no sea cero y que el agente esté activo
            if (barrierPoint != Vector3.zero && agent.isOnNavMesh)
            {
                try
                {
                    agent.SetDestination(barrierPoint);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Error al establecer destino: " + e.Message);
                }
            }
            
            if (isInBarrier && !isAttackingBarrier)
            {
                isAttackingBarrier = true;
                vignetteController.SumEnemyAttacking();
                canAttack = true;
            }
            else if (!isInBarrier && isAttackingBarrier)
            {
                isAttackingBarrier = false;
                vignetteController.DecreaseEnemiesAttacking();
                canAttack = false;
            }
        }

        // Solo dibujar líneas de debug si el agente tiene una ruta válida
        if (agent.hasPath)
        {
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.cyan);
            }
        }

        if ((isAttackingBarrier || isAttackingDefense) && canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                if (isAttackingBarrier)
                {
                    AttackBarrier();
                }
                if (isAttackingDefense && targetDefense != null)
                {
                    AttackDefense();
                }
                attackTimer = 0f;
            }
        }

        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                ResetSpeed();
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        barrierPoint = target;
        
        // Solo establecer el destino si el agente está activo
        if (agent != null && agent.isOnNavMesh && agent.enabled && !isDying)
        {
            try
            {
                agent.SetDestination(target);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error al establecer destino en SetTarget: " + e.Message);
            }
        }
    }

    public Vector3 GetBarrierPoint()
    {
        return barrierPoint;
    }

    private void AttackBarrier()
    {
        if (targetBarrier != null)
        {
            targetBarrier.TakeDamage(damageAmount);

            Damageable damageable = GetComponent<Damageable>();
            if (damageable != null)
            {
                float fatalDamage = damageable.health;
                damageable.ApplyDamage(fatalDamage);
            }
        }
    }

    private void AttackDefense()
    {
        if (targetDefense != null)
        {
            Debug.Log($"{gameObject.name} ataca la defensa causando {damageAmount} de daño.");
            targetDefense.TryGetComponent<Damageable>(out Damageable damageable);
            if (damageable != null)
            {
                damageable.ApplyDamage(damageAmount, gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // No procesar si está muriendo
        if (isDying) return;
        
        if (other.CompareTag("WarningZone"))
        {
            isInBarrier = false;
        }
        if (other.CompareTag("Defense"))
        {
            if (!isAttackingDefense)
            {
                isAttackingDefense = true;
                targetDefense = other.GetComponent<Defense>();
                
                // Verificar que el agente esté activo
                if (agent != null && agent.isOnNavMesh && agent.enabled)
                {
                    try
                    {
                        agent.SetDestination(targetDefense.transform.position);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Error al establecer destino en OnTriggerEnter: " + e.Message);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // No procesar si está muriendo
        if (isDying) return;
        
        if (other.CompareTag("WarningZone"))
        {
            isInBarrier = true;
        }
    }

    public void ApplySlow(float slowAmount, float slowDuration)
    {
        // No aplicar ralentización si está muriendo o el agente no está activo
        if (isDying || agent == null || !agent.enabled) return;
        
        float slowMultiplier = 1f - (slowAmount / 100f);
        agent.speed = Mathf.Max(baseSpeed * slowMultiplier, 0.1f);

        Debug.Log($"{gameObject.name} ha sido ralentizado en {slowAmount}% durante {slowDuration} segundos. (Nueva Vel: {agent.speed})");

        isSlowed = true;
        slowTimer = slowDuration;

        if (agent.isOnNavMesh)
        {
            agent.velocity = agent.desiredVelocity.normalized * agent.speed;
            agent.isStopped = false;
        }
    }

    void ResetSpeed()
    {
        if (agent != null && agent.enabled)
        {
            agent.speed = baseSpeed;
        }
        isSlowed = false;
    }

    public Vector3 GetVelocity()
    {
        return agent != null && agent.enabled ? agent.velocity : Vector3.zero;
    }

    private void HandleDefenseSpawned(Defense newDefense)
    {
        // No procesar si está muriendo
        if (isDying) return;
        
        if (newDefense != null && Vector3.Distance(transform.position, newDefense.transform.position) <= aggroRange + newDefense.attractionRange)
        {
            if (isAttackingDefense && targetDefense != null && (newDefense.priority > targetDefense.priority))
            {
                targetDefense = newDefense;
                isAttackingDefense = true;
                
                if (agent != null && agent.isOnNavMesh && agent.enabled)
                {
                    try
                    {
                        agent.SetDestination(newDefense.transform.position);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Error al establecer destino en HandleDefenseSpawned: " + e.Message);
                    }
                }
            }
            else if (isAttackingBarrier && (newDefense.priority > 2))
            {
                targetDefense = newDefense;
                isAttackingDefense = true;
                isAttackingBarrier = false;
                
                if (vignetteController != null)
                {
                    vignetteController.DecreaseEnemiesAttacking();
                }
                
                if (agent != null && agent.isOnNavMesh && agent.enabled)
                {
                    try
                    {
                        agent.SetDestination(newDefense.transform.position);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Error al establecer destino en HandleDefenseSpawned (2): " + e.Message);
                    }
                }
            }
        }
    }

    private void HandleDefenseDestroyed(Defense destroyedDefense)
    {
        // No procesar si está muriendo
        if (isDying) return;
        
        if (targetDefense == destroyedDefense)
        {
            targetDefense = null;
            Defense defenseInRange = FindNearestDefense();
            
            if (defenseInRange != null)
            {
                targetDefense = defenseInRange;
                isAttackingDefense = true;
                
                if (agent != null && agent.isOnNavMesh && agent.enabled)
                {
                    try
                    {
                        agent.SetDestination(defenseInRange.transform.position);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Error al establecer destino en HandleDefenseDestroyed: " + e.Message);
                    }
                }
            }
            else
            {
                isAttackingDefense = false;
                
                if (agent != null && agent.isOnNavMesh && agent.enabled && barrierPoint != Vector3.zero)
                {
                    try
                    {
                        agent.SetDestination(barrierPoint);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Error al establecer destino en HandleDefenseDestroyed (2): " + e.Message);
                    }
                }
            }
        }
    }

    private Defense FindNearestDefense()
    {
        // No buscar si está muriendo
        if (isDying) return null;
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange);
        Defense nearestDefense = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Defense"))
            {
                Defense defense = col.GetComponent<Defense>();
                if (defense != null)
                {
                    float distance = Vector3.Distance(transform.position, defense.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDefense = defense;
                        nearestDistance = distance;
                    }
                }
            }
        }

        return nearestDefense;
    }

    // Método para ser llamado cuando el enemigo comienza a morir
    public void StartDying()
    {
        isDying = true;
        
        // Desactivar NavMeshAgent
        if (agent != null)
        {
            agent.isStopped = true;
            agent.updatePosition = false;
            agent.updateRotation = false;
            // No deshabilitamos el componente completo para evitar errores en otros scripts
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}