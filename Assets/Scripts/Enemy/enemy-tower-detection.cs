using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(EnemyAI))]
public class EnemyTowerDetector : MonoBehaviour
{
    public float detectionRadius = 5f;
    public bool preferDefenses = false; // Si es true, priorizará atacar defensas sobre seguir su camino

    private SphereCollider detectionCollider;
    private EnemyAI enemyAI;
    private Defense currentTarget;

    private void Start()
    {
        // Configurar el collider de detección
        detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.radius = detectionRadius;
        detectionCollider.isTrigger = true;

        enemyAI = GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defense"))
        {
            Defense defense = other.GetComponent<Defense>();
            if (defense != null && (currentTarget == null || preferDefenses))
            {
                // Atacar la defensa
                currentTarget = defense;
                enemyAI.AttackDefenseTarget(defense);

                // Suscribirse al evento de muerte de la defensa
                var damageable = defense.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.OnDeath += OnDefenseDestroyed;
                }
            }
        }
    }

    private void OnDefenseDestroyed()
    {
        if (currentTarget != null)
        {
            // Desuscribirse del evento de muerte
            var damageable = currentTarget.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.OnDeath -= OnDefenseDestroyed;
            }

            // Dejar de atacar la defensa y continuar con el camino original
            enemyAI.StopAttackingDefense();
            currentTarget = null;
        }
    }

    private void OnDestroy()
    {
        // Limpieza de eventos al destruir el objeto
        if (currentTarget != null)
        {
            var damageable = currentTarget.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.OnDeath -= OnDefenseDestroyed;
            }
        }
    }
}