using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployButton : MonoBehaviour
{
    public GameObject unitPrefab; // Assign the prefab to this button

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnDeployButtonClick);
    }

    private void OnDeployButtonClick()
    {
        DeployManager.Instance.StartDeployment(unitPrefab);
    }
}