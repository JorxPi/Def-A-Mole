using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance; 

    public int CurrentGold { get; private set; } = 100;

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

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        OnGoldChanged?.Invoke(CurrentGold);
    }

    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            OnGoldChanged?.Invoke(CurrentGold);
            return true;
        }
        else
        {
            return false;
        }
    }
}
