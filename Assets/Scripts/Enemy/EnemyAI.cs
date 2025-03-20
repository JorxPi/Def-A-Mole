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

        agent.SetDestination(barrierPoint);

        GameObject barrierObject = GameObject.FindWithTag("Barrier");
        targetBarrier = barrierObject.GetComponent<Barrier>();

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
        
        if(isAttackingDefense && targetDefense != null)
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
            agent.SetDestination(barrierPoint);
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

        for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.cyan);
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
    }

    public Vector3 GetBarrierPoint()
    {
        return barrierPoint;
    }

    private void AttackBarrier()
    {
        targetBarrier.TakeDamage(damageAmount);

        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            float fatalDamage = damageable.health;
            damageable.ApplyDamage(fatalDamage);
        }
    }

    private void AttackDefense()
    {
        if (targetDefense != null)
        {
            Debug.Log($"{gameObject.name} ataca la defensa causando {damageAmount} de daño.");
            targetDefense.TryGetComponent<Damageable>(out Damageable damageable);
            damageable.ApplyDamage(damageAmount, gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
                agent.SetDestination(targetDefense.transform.position);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WarningZone"))
        {
            isInBarrier = true;
        }
    }


    public void ApplySlow(float slowAmount, float slowDuration)
    {
        float slowMultiplier = 1f - (slowAmount / 100f);
        agent.speed = Mathf.Max(baseSpeed * slowMultiplier, 0.1f);

        Debug.Log($"{gameObject.name} ha sido ralentizado en {slowAmount}% durante {slowDuration} segundos. (Nueva Vel: {agent.speed})");

        isSlowed = true;
        slowTimer = slowDuration;

        agent.velocity = agent.desiredVelocity.normalized * agent.speed;
        agent.isStopped = false;
    }

    void ResetSpeed()
    {
        agent.speed = baseSpeed;
        isSlowed = false;
    }

    public Vector3 GetVelocity()
    {
        return agent.velocity;
    }

    private void HandleDefenseSpawned(Defense newDefense)
    {
        if (targetDefense == null && Vector3.Distance(transform.position, newDefense.transform.position) <= aggroRange+newDefense.attractionRange)
        {
            if (isAttackingDefense && (newDefense.priority > targetDefense.priority))
                {
                targetDefense = newDefense;
                isAttackingDefense = true;
                agent.SetDestination(newDefense.transform.position);
            }
            if (isAttackingBarrier && (newDefense.priority > 2))
            {
                targetDefense = newDefense;
                isAttackingDefense = true;
                isAttackingBarrier = false;
                vignetteController.DecreaseEnemiesAttacking();
                agent.SetDestination(newDefense.transform.position);
            }
        }
    }

    private void HandleDefenseDestroyed(Defense destroyedDefense)
    {
        if (targetDefense == destroyedDefense)
        {
            targetDefense = null;
            Defense defenseInRange = FindNearestDefense();
            if (defenseInRange != null)
            {
                targetDefense = defenseInRange;
                isAttackingDefense = true;
                agent.SetDestination(defenseInRange.transform.position);
            }
            else
            {
                isAttackingDefense = false;
                agent.SetDestination(barrierPoint);
            }
        }
    }

    private Defense FindNearestDefense()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}