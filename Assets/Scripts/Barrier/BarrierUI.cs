using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarrierUI : MonoBehaviour
{
    public TMP_Text healthText; // Asigna esto en el Inspector
    public Slider healthBar;
    public Barrier barrier; // Asigna esto en el Inspector

    private void OnEnable()
    {
        if (barrier != null)
        {
            barrier.onHealthChanged += UpdateHealthDisplay;
        }
        else
        {
            //Debug.LogError("Barrier no est� asignado en el Inspector.");
        }
    }

    private void OnDisable()
    {
        if (barrier != null)
        {
            barrier.onHealthChanged -= UpdateHealthDisplay;
        }
    }

    private void Start()
    {
        if (barrier != null)
        {
            UpdateHealthDisplay(barrier.GetCurrentHealth());
        }
    }

    private void UpdateHealthDisplay(float currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}";
        }
        else
        {
            Debug.LogError("healthText no est� asignado en el Inspector.");
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth / barrier.maxHealth; 
        }
    }
}
