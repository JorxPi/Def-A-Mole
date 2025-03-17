using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenseInfoUI : MonoBehaviour
{
    public static DefenseInfoUI instance;

    public GameObject panel;
    public TMP_Text defenseNameText;
    public TMP_Text defenseStatsText;
    public TMP_Text defenseDescriptionText;

    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void ShowInfo(DefenseData defenseData)
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
            defenseNameText.text = defenseData.defenseName;
            defenseDescriptionText.text = defenseData.description;
        }
    }

    public void HideInfo()
    {
        panel.SetActive(false);
    }
}