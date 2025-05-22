using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DeployButton : MonoBehaviour
{
    public DefenseData defenseData;
    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>(); 
        btn.onClick.AddListener(OnDeployButtonClick);

        if (!LevelManager.Instance.unlockedDefenses.Contains(defenseData))
        {
            btn.interactable = false; 
        }
    }

    private void OnDeployButtonClick()
    {
        DeployManager.Instance.StartDeployment(defenseData);
        AudioManager.Instance.PlayButtonClick(0.5f);
    }
}