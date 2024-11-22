using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployButton : MonoBehaviour
{
    public DefenseData defenseData;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnDeployButtonClick);
    }

    private void OnDeployButtonClick()
    {
        DeployManager.Instance.StartDeployment(defenseData);
    }
}