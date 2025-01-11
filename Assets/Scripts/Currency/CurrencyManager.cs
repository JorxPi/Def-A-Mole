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

    private int currentGold;

    public event Action<int> OnGoldChanged; // Event to notify when gold changes

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
        OnGoldChanged?.Invoke(currentGold); // Notify subscribers of the initial gold value
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
            OnGoldChanged?.Invoke(currentGold); // Notify subscribers of the new gold value
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold); // Notify subscribers of the new gold value
    }
}
