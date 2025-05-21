using System.Collections;
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
    private float distanceToBarrier;
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

    private EnemyAnimatorController animController;

    [Header("Animation Timing")]
    public AnimationClip attackLoopClip;

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
        animController = GetComponent<EnemyAnimatorController>();

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

        if (attackLoopClip != null)
        {
            float clipLength = attackLoopClip.length;
            float speed = clipLength / attackInterval;
            animController.animator.SetFloat("attackSpeed", speed);
        }
        else
        {
            Debug.LogWarning("Assign the looping attack clip in EnemyAI.");
        }
    }

    private void Update()
    {
        distanceToBarrier =  Vector3.Distance(transform.position, barrierPoint);

        //Debug.Log($"Distance to barrier: {distanceToBarrier}");

        if (isAttackingDefense && targetDefense != null)
        {
            float distanceToDefense = Vector3.Distance(transform.position, targetDefense.transform.position);
            if (distanceToDefense <= attackRange + 3.5)
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

            if(distanceToBarrier <= attackRange && !isAttackingBarrier)
            {
                isAttackingBarrier = true;
                vignetteController.SumEnemyAttacking();
                canAttack = true;
            }
            else if(distanceToBarrier > attackRange && isAttackingBarrier)
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

        bool shouldBeAttacking = (isAttackingBarrier || isAttackingDefense) && canAttack;

        // Tick attack timer
        if (shouldBeAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                // Play attack animation
                animController.PlayAttack();

                // Apply damage
                if (isAttackingBarrier)
                {
                    AttackBarrier();
                }
                else if (isAttackingDefense && targetDefense != null)
                {
                    AttackDefense();
                }

                attackTimer = 0f;
            }
        }
        else
        {
            attackTimer = 0f; // reset if not in range
        }

        // Proceed with attack logic if allowed
        if (shouldBeAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                if (isAttackingBarrier)
                {
                    AttackBarrier();
                }
                else if (isAttackingDefense && targetDefense != null)
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

    // Animation
    public void Die()
    {
        if (isAttackingBarrier)
        {
            vignetteController.DecreaseEnemiesAttacking();
        }

        agent.isStopped = true;
        animController.PlayDead();
    }

    // Animation finish
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
        targetBarrier.TakeDamage(1f);

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

    public void ApplySlow(float slowAmount, float slowDuration)
    {
        float slowMultiplier = 1f - (slowAmount / 100f);
        agent.speed = Mathf.Max(baseSpeed * slowMultiplier, 0.1f);

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