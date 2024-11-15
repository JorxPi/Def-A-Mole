using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    public Text goldText;

    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged += UpdateGoldUI;
        }
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnGoldChanged -= UpdateGoldUI;
        }
    }

    private void UpdateGoldUI(int currentGold)
    {
        goldText.text = "Gold: " + currentGold;
    }
}
