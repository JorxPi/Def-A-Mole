using UnityEngine;
using System.Collections.Generic;

public class EnemyTargeting : MonoBehaviour
{
    [SerializeField] private bool targetsTowers = true;

    private EnemyAI enemyAI;
    private Transform currentTarget;
    private Vector3 edgeTarget;
    private float detectionRange = 10f;
    private float stopDistance = 1.5f;
    private float safetyMargin = 0.5f;
    private LevelManager levelManager;
    private GameObject[] defenses;
    private bool isAttackingDefense = false;

    // Variables para detectar muros
    private GameObject wallTarget;
    private float wallDetectionDistance = 1.5f;

    // Variables para el movimiento suave
    private Vector3 currentVelocity;
    private float smoothTime = 0.5f;
    private float rotationSpeed = 5f;
    private Vector3 targetPosition;

    // Variables para animación y ataque
    private Animator animator;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackInterval = 1f;
    private float attackDamage;
    private Damageable currentTargetDamageable;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        levelManager = FindObjectOfType<LevelManager>();
        animator = GetComponent<Animator>();
        attackDamage = enemyAI.damageAmount;

        UpdateEdgeTarget();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isAttackingDefense)
        {
            FindNearestDefenseOrWall();
            SetWalkingAnimation(true);
        }
        else if (currentTarget == null)
        {
            isAttackingDefense = false;
            isAttacking = false;
            SetWalkingAnimation(true);
            ReturnToEdge();
        }

        UpdateMovement();
        UpdateAttack();
    }

    void FindNearestDefenseOrWall()
    {
        // Si `targetsTowers` es falso, ignoramos las defensas
        if (targetsTowers)
        {
            defenses = GameObject.FindGameObjectsWithTag("Defense");
        }
        else
        {
            defenses = new GameObject[0]; // Vacío si no se deben atacar defensas
        }

        // Buscar el muro más cercano (tag "Wall")
        GameObject nearestWall = GameObject.FindGameObjectWithTag("Wall");
        float distanceToWall = nearestWall != null ? Vector3.Distance(transform.position, nearestWall.transform.position) : float.MaxValue;

        // Buscar la defensa más cercana si `targetsTowers` es verdadero
        float nearestDistance = float.MaxValue;
        Transform nearestDefense = null;

        foreach (GameObject defense in defenses)
        {
            float distance = Vector3.Distance(transform.position, defense.transform.position);
            if (distance < detectionRange && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestDefense = defense.transform;
            }
        }

        // Decidir entre atacar una torre o el muro
        if (distanceToWall <= wallDetectionDistance)
        {
            currentTarget = nearestWall.transform;
            currentTargetDamageable = nearestWall.GetComponent<Damageable>();
            isAttackingDefense = true;
            UpdateTargetPosition();
        }
        else if (nearestDefense != null)
        {
            currentTarget = nearestDefense;
            currentTargetDamageable = nearestDefense.GetComponent<Damageable>();
            isAttackingDefense = true;
            UpdateTargetPosition();
        }
        else
        {
            currentTarget = null;
            currentTargetDamageable = null;
            isAttackingDefense = false;
        }
    }

    void UpdateEdgeTarget()
    {
        if (levelManager != null)
        {
            edgeTarget = transform.position.normalized * levelManager.mapRadius;
            edgeTarget.y = 0;
            targetPosition = edgeTarget;
        }
    }

    void ReturnToEdge()
    {
        UpdateEdgeTarget();
        UpdateTargetPosition();
        currentTargetDamageable = null;
    }

    void UpdateTargetPosition()
    {
        Vector3 targetPos = isAttackingDefense ? currentTarget.position : edgeTarget;
        Vector3 directionToTarget = (targetPos - transform.position).normalized;

        float totalStopDistance = stopDistance + safetyMargin;
        targetPosition = targetPos - directionToTarget * totalStopDistance;
        targetPosition.y = transform.position.y;

        enemyAI.SetTarget(targetPosition);
    }

    void UpdateMovement()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        bool isInAttackRange = distanceToTarget <= stopDistance + safetyMargin;

        // Actualizar estado de ataque
        if (isInAttackRange)
        {
            if (isAttackingDefense && currentTarget != null)
            {
                isAttacking = true;
                SetWalkingAnimation(false);
                SetAttackAnimation(true);
            }
            else
            {
                isAttacking = false;
                SetAttackAnimation(false);
                SetWalkingAnimation(true);
            }
        }
        else
        {
            isAttacking = false;
            SetAttackAnimation(false);
            SetWalkingAnimation(true);
        }

        // Actualizar rotación
        if (isAttacking && Vector3.Distance(transform.position, edgeTarget) <= stopDistance + safetyMargin)
        {
            Vector3 outwardDirection = (transform.position - Vector3.zero).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(outwardDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else if (currentTarget != null)
        {
            Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        if (isAttackingDefense && currentTarget != null)
        {
            UpdateTargetPosition();
        }
    }

    void UpdateAttack()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                if (currentTargetDamageable != null)
                {
                    currentTargetDamageable.ApplyDamage(attackDamage);
                }
                attackTimer = 0f;
            }
        }
    }

    void SetWalkingAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }

    void SetAttackAnimation(bool isAttacking)
    {
        if (animator != null)
        {
            animator.SetBool("isAttacking", isAttacking);
        }
    }

    public void SetDetectionRange(float range)
    {
        detectionRange = range;
    }

    public void SetStopDistance(float distance)
    {
        stopDistance = distance;
    }

    public void SetSmoothTime(float time)
    {
        smoothTime = time;
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void SetSafetyMargin(float margin)
    {
        safetyMargin = margin;
    }

    public void SetAttackInterval(float interval)
    {
        attackInterval = interval;
    }
}
