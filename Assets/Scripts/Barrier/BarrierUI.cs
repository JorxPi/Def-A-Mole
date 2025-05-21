using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarrierUI : MonoBehaviour
{
    public TMP_Text healthText;
    public Barrier barrier;
    public Slider healthBar;
    public Image fillImage;


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

        if (currentHealth > 5)
        {
            fillImage.color = Color.green;
        }
        else if (currentHealth > 3)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red;
        }
    }
}


