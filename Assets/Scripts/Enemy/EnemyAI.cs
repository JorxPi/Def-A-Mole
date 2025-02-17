using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
    [Header("Combat Settings")]
    public float damageAmount = 10f;
    public float attackInterval = 1f;
    public float baseSpeed = 3.5f;
    public float towerDetectionRange = 1f;
    public float attackRange = 2f; // Rango para atacar a las defensas

    [Header("References")]
    private Barrier targetBarrier;
    private EnemyCombatAnimation combatAnimation;
    private NavMeshAgent agent;

    [Header("State Variables")]
    private Vector3 originalTargetPoint;
    private Vector3 currentTargetPoint;
    private bool hasTarget = false;
    private bool isAttacking = false;
    private bool isAttackingDefense = false;
    private float attackTimer;
    private bool isSlowed = false;
    private float slowTimer = 0f;
    private Defense currentTargetDefense;

    void Start()
    {
        // Initialize components
        agent = GetComponent<NavMeshAgent>();
        combatAnimation = GetComponent<EnemyCombatAnimation>();
        agent.speed = baseSpeed;
        agent.stoppingDistance = attackRange; // Establecer la distancia de frenado

        // Find and setup barrier
        GameObject barrierObject = GameObject.FindWithTag("Barrier");
        if (barrierObject != null)
        {
            targetBarrier = barrierObject.GetComponent<Barrier>();
            if (targetBarrier == null)
            {
                Debug.LogError("El objeto con tag 'Barrier' no tiene un componente Barrier.");
            }
        }
    }

    private void Update()
    {
        UpdateMovement();
        UpdateCombat();
        UpdateSlowEffect();
    }

    private void UpdateMovement()
    {
        if (combatAnimation != null)
        {
            combatAnimation.SetWalking(agent.velocity.magnitude > 0.1f);
        }

        // Si está atacando una defensa, mantener la distancia
        if (isAttackingDefense && currentTargetDefense != null)
        {
            float distanceToDefense = Vector3.Distance(transform.position, currentTargetDefense.transform.position);
            if (distanceToDefense <= attackRange)
            {
                agent.SetDestination(transform.position); // Mantener posición
                transform.LookAt(currentTargetDefense.transform); // Mirar hacia la defensa
            }
            else
            {
                agent.SetDestination(currentTargetDefense.transform.position);
            }
        }
        else if (!isAttacking) // Si no está atacando nada, seguir el camino original
        {
            agent.SetDestination(currentTargetPoint);
        }
    }

    private void UpdateCombat()
    {
        attackTimer += Time.deltaTime;

        if (isAttacking && attackTimer >= attackInterval)
        {
            AttackBarrier();
            attackTimer = 0f;
        }
        else if (isAttackingDefense && currentTargetDefense != null && attackTimer >= attackInterval)
        {
            float distanceToDefense = Vector3.Distance(transform.position, currentTargetDefense.transform.position);
            if (distanceToDefense <= attackRange)
            {
                AttackDefense();
                attackTimer = 0f;
            }
        }
    }

    private void AttackDefense()
    {
        if (currentTargetDefense != null)
        {
            if (combatAnimation != null)
            {
                combatAnimation.PlayAttackAnimation();
            }

            var damageable = currentTargetDefense.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.ApplyDamage(damageAmount);
                Debug.Log($"Atacando defensa. Daño: {damageAmount}");
            }
        }
    }

    public void SetTarget(Vector3 target)
    {
        originalTargetPoint = target;
        currentTargetPoint = target;
        hasTarget = true;
    }

    private void AttackBarrier()
    {
        if (targetBarrier != null)
        {
            if (combatAnimation != null)
            {
                combatAnimation.PlayAttackAnimation();
            }
            targetBarrier.TakeDamage(damageAmount);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WarningZone"))
        {
            isAttacking = false;
            if (combatAnimation != null)
            {
                combatAnimation.SetWalking(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WarningZone"))
        {
            isAttacking = true;
        }
    }

    public void ApplySlow(float slowAmount, float slowDuration)
    {
        if (!isSlowed)
        {
            agent.speed = baseSpeed - slowAmount;
            isSlowed = true;
        }
        slowTimer = slowDuration;
    }

    void ResetSpeed()
    {
        agent.speed = baseSpeed;
        isSlowed = false;
    }

    private void UpdateSlowEffect()
    {
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                ResetSpeed();
            }
        }
    }

    public void AttackDefenseTarget(Defense defense)
    {
        if (defense != null)
        {
            currentTargetDefense = defense;
            isAttackingDefense = true;
            currentTargetPoint = defense.transform.position;
            attackTimer = attackInterval;
        }
    }

    public void StopAttackingDefense()
    {
        isAttackingDefense = false;
        currentTargetDefense = null;
        currentTargetPoint = originalTargetPoint;
    }

    public bool IsAttacking()
    {
        return isAttacking || isAttackingDefense;
    }

    public Vector3 GetTargetPosition()
    {
        return originalTargetPoint;
    }
}