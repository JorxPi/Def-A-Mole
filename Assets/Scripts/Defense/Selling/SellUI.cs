using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellUI : MonoBehaviour
{
    public static SellUI Instance;

    public GameObject sellPanel;
    public Button sellButton;
    public TMP_Text sellButtonText;
    public TMP_Text sellInfoText;

    private GameObject selectedDefense;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        HideSellPanel();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            HideSellPanel();
        }
    }

    public void ShowSellPanel(GameObject defense)
    {
        if (!DayNightManager.Instance || DayNightManager.Instance.IsNightMode)
        {
            Debug.Log("Selling is only allowed during daytime.");
            return;
        }

        selectedDefense = defense;

        Defense def = defense.GetComponent<Defense>();
        if (def != null && def.baseStats != null)
        {
            int cost = def.baseStats.cost;
            int refund = Mathf.FloorToInt(cost * SellManager.Instance.refundRatio);

            sellButtonText.text = $"Sell for {refund} Gold";
            sellInfoText.text = $"Refund 75% of original cost ({cost}g)";
            sellButton.interactable = true;
        }

        sellPanel.SetActive(true);
    }

    public void HideSellPanel()
    {
        selectedDefense = null;
        sellPanel.SetActive(false);
    }

    public void OnSellButtonClicked()
    {
        if (selectedDefense != null)
        {
            SellManager.Instance.SellDefense(selectedDefense);
            UpgradeUI.Instance.HideUpgradePanel();

            HideSellPanel();
        }
    }
}

