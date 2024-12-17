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
        CurrencyManager.Instance.OnGoldChanged += UpdateCurrencyDisplay;
    }

    private void OnDisable()
    {
        CurrencyManager.Instance.OnGoldChanged -= UpdateCurrencyDisplay;
    }

    private void Start()
    {
        // Initialize the display with the current gold amount
        UpdateCurrencyDisplay(CurrencyManager.Instance.startingGold);
    }

    private void UpdateCurrencyDisplay(int goldAmount)
    {
        currencyText.text = $"Gold: {goldAmount}";
    }
}
