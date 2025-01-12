using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public delegate void OnHealthChanged(float currentHealth);
    public event OnHealthChanged onHealthChanged;

    private GameOverController gameOverController;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
        gameOverController = FindObjectOfType<GameOverController>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            // Barrier destroyed
            currentHealth = 0f;

           
            if (gameOverController != null)
            {
                gameOverController.ShowGameOver();
            }
            else{
                Debug.LogWarning("GameOverController is null");
            }
        }
        onHealthChanged?.Invoke(currentHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
