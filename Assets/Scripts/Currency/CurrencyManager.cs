using System.Collections;
using System.Collections.Generic;
using System; // For the Action delegate
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("Starting Gold")]
    public int startingGold = 100;

    [Header("UI Reference")]
    public TMP_Text currencyText; 

    private int currentGold;

    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentGold = startingGold;
        UpdateCurrencyUI();
    }

    public bool HasEnoughGold(int amount)
    {
        return currentGold >= amount;
    }

    public void SpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            currentGold -= amount;
            UpdateCurrencyUI();
            OnGoldChanged?.Invoke(currentGold);
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateCurrencyUI();
        OnGoldChanged?.Invoke(currentGold);
    }

    private void UpdateCurrencyUI()
    {
        currencyText.text = $"Gold: {currentGold}";
    }
}
