using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarrierUI : MonoBehaviour
{
    public TMP_Text healthText;
    public Image healthFillImage;
    public Barrier barrier;         

    private float lastHealth = -1f;

    private void Start()
    {
        UpdateHealthUI(barrier.GetCurrentHealth());

        barrier.onHealthChanged += HandleHealthChanged;
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

        float normalized = Mathf.Clamp01(currentHealth / barrier.maxHealth);
        healthFillImage.fillAmount = normalized;
        //healthBar.value = normalized;
        // get component image . fillAmount
    }
}


