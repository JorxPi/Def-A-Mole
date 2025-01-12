using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public float damageAmount = 10f;
    public float attackInterval = 1f;
    public float baseSpeed = 3.5f;

    private Barrier targetBarrier;

    private Vector3 targetPoint;
    private bool hasTarget = false;
    private bool isAttacking = false;

    private NavMeshAgent agent;
    private float attackTimer;

    private bool isSlowed = false;
    private float slowTimer = 0f;

    private bool isAttackingTower = false;
    Defense targetDefense;
    private Vector3 towerPoint;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;

        GameObject barrierObject = GameObject.FindWithTag("Barrier");
        if (barrierObject != null)
        {
            targetBarrier = barrierObject.GetComponent<Barrier>();
            if (targetBarrier == null)
            {
                Debug.LogError("El objeto con tag 'Barrier' no tiene un componente Barrier.");
            }
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con el tag 'Barrier' en la escena.");
        }
    }

    private void Update()
    {

        agent.destination = targetPoint;

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                AttackBarrier();
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

        if (isAttackingTower)
        {
            if (Vector3.Distance(transform.position, towerPoint) <= 0.5f)
            {
                
            }
        }

        /*
        if (hasTarget)
        {
            // Establecer destino del agente
            agent.destination = targetPoint;

            // Comprobar si está cerca del objetivo
            if (Vector3.Distance(transform.position, targetPoint) <= stopDistance)
            {
                StopAtEdge();
            }
        }

*/
    }

    public void SetTarget(Vector3 target)
    {
        targetPoint = target;
        hasTarget = true;
    }


    private void AttackBarrier()
    {
        // Aquí se debería restar vida a la barrera (esto se conecta con el sistema de vida de la barrera)
        Debug.Log($"{gameObject.name} está atacando la barrera y causa {damageAmount} de daño.");
        targetBarrier.TakeDamage(damageAmount);

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WarningZone"))
        {
            isAttacking = true;
            Debug.Log($"{gameObject.name} ha entrado de la zona de advertencia y comienza a atacar.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WarningZone"))
        {
            isAttacking = false;
        }
    }


    public void ApplySlow(float slowAmount, float slowDuration)
    {
        if(!isSlowed) {
            Debug.Log($"{gameObject.name} ha sido ralentizado por {slowAmount * 100}% durante {slowDuration} segundos.");
            agent.speed = agent.speed - slowAmount;
            isSlowed = true;
        }
        else
        {
            slowTimer = slowDuration;
        }
    }

    void ResetSpeed()
    {
        agent.speed = baseSpeed;
        isSlowed = false;
    }

    public void SetTowerTarget(Vector3 tower)
    {
        towerPoint = tower;
    }

    public void AttackTower(Defense defense)
    {
        Damageable damageable = defense.GetComponent<Damageable>();
        if (damageable != null)
        {
            Defense targetDefense = defense;
            isAttackingTower = true;

        }
    }
}