using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpgradeDefense(GameObject defenseObject)
    {
        UpgradeHandler upgradeHandler = defenseObject.GetComponent<UpgradeHandler>();

        if (upgradeHandler != null)
        {
            bool success = upgradeHandler.Upgrade();
            if (!success)
            {
                Debug.Log("Upgrade failed. Either max level reached or not enough gold.");
            }
        }
        else
        {
            Debug.LogWarning("This defense cannot be upgraded. Missing UpgradeHandler.");
        }
    }
}
