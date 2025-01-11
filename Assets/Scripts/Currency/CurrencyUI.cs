using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text currencyText; // Assign this in the Inspector

    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged += UpdateCurrencyDisplay;
        }
        else
        {
            Debug.LogWarning("CurrencyManager.Instance is null in OnEnable. Ensure CurrencyManager is initialized before CurrencyUI.");
        }
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged -= UpdateCurrencyDisplay;
        }
    }

    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            // Initialize the display with the current gold amount
            UpdateCurrencyDisplay(CurrencyManager.Instance.startingGold);
        }
        else
        {
            Debug.LogError("CurrencyManager.Instance is null in Start. Ensure CurrencyManager is added to the scene.");
        }
    }

    private void UpdateCurrencyDisplay(int goldAmount)
    {
        if (currencyText != null)
        {
            currencyText.text = $"Gold: {goldAmount}";
        }
        else
        {
            Debug.LogError("currencyText is not assigned in the Inspector.");
        }
    }
}
