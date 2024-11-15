using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el componente Animator del GameObject.
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking); // Cambia el estado de la animación basado en el parámetro.
    }
}
