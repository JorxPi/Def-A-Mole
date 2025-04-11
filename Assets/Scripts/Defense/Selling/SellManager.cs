using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public static SellManager Instance;

    [Range(0f, 1f)]
    public float refundRatio = 0.75f; // 75% refund

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SellDefense(GameObject defenseObject)
    {
        Defense defense = defenseObject.GetComponent<Defense>();

        if (defense != null && defense.baseStats != null)
        {
            int baseCost = defense.baseStats.cost;
            int refund = Mathf.FloorToInt(baseCost * refundRatio);

            CurrencyManager.Instance.AddGold(refund);
            Destroy(defenseObject);

            Debug.Log($"Defense sold for {refund} gold.");
        }
        else
        {
            Debug.LogWarning("Sell failed. Missing Defense or baseStats.");
        }
    }
}


