using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarrierUI : MonoBehaviour
{
    public TMP_Text healthText;
    public Barrier barrier;
    public Slider healthBar;


    private float lastHealth = -1f;

    private void Start()
    {
        UpdateHealthUI(barrier.GetCurrentHealth());

        barrier.onHealthChanged += HandleHealthChanged;

        healthBar.value = healthBar.maxValue;
    }

    private void OnDestroy()
    {
        if (barrier != null)
        {
            barrier.onHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float newHealth)
    {
        if (!Mathf.Approximately(newHealth, lastHealth))
        {
            UpdateHealthUI(newHealth);
        }
    }

    private void UpdateHealthUI(float currentHealth)
    {
        lastHealth = currentHealth;

        healthText.text = $"{Mathf.RoundToInt(currentHealth)}";

        healthBar.value = currentHealth;
    }
}


