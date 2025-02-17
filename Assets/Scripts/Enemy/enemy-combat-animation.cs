using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyCombatAnimation : MonoBehaviour
{
    private Animator animator;
    private EnemyAI enemyAI;

    // Nombres de los parámetros en el Animator
    private const string ATTACK_TRIGGER = "Attack";
    private const string IS_WALKING = "isWalking";

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();

        if (enemyAI == null)
        {
            Debug.LogError("No se encontró el componente EnemyAI");
            return;
        }
    }

    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(ATTACK_TRIGGER);
        }
    }

    public void SetWalking(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool(IS_WALKING, isWalking);
        }
    }
}
