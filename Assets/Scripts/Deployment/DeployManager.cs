using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployManager : MonoBehaviour
{
    public static DeployManager Instance { get; private set; }

    public GameObject map; // Reference to the map GameObject
    public LayerMask mapLayer; // Assign the "Map" layer in the Inspector

    private bool isDeploying = false;
    private DefenseData currentDefenseData;

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

    private void Update()
    {
        if (isDeploying)
        {
            HandleDeployment();
        }
    }

    public void StartDeployment(DefenseData defenseData)
    {
        if (CurrencyManager.Instance.HasEnoughGold(defenseData.cost))
        {
            currentDefenseData = defenseData;
            isDeploying = true;
        }
        else
        {
            Debug.Log("Not enough gold to deploy this defense!");
        }
    }

    private void HandleDeployment()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CancelDeployment();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            PlaceDefense();
        }
    }

    private void PlaceDefense()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // Raycast only hits the "Map" layer
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mapLayer))
        {
            // Ensure placement is valid (not too close to another defense)
            if (!IsDefenseInLocation(hit.point))
            {
                // Adjust for "Feet" offset if necessary
                Transform feetTransform = currentDefenseData.prefab.transform.Find("Feet");
                Vector3 feetOffset = Vector3.zero;

                if (feetTransform != null)
                {
                    feetOffset = currentDefenseData.prefab.transform.position - feetTransform.position;
                }

                // Final position for the defense
                Vector3 targetPosition = hit.point + feetOffset;

                // Instantiate the defense
                Instantiate(currentDefenseData.prefab, targetPosition, Quaternion.identity);

                // Deduct the cost and finalize deployment
                CurrencyManager.Instance.SpendGold(currentDefenseData.cost);
                Debug.Log($"Defense placed at: {targetPosition}");
                isDeploying = false;
            }
            else
            {
                Debug.Log("Placement blocked by another defense.");
                CancelDeployment();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit the map.");
            CancelDeployment();
        }
    }

    private void CancelDeployment()
    {
        isDeploying = false;
        currentDefenseData = null;
        Debug.Log("Deployment cancelled.");
    }

    private bool IsDefenseInLocation(Vector3 position)
    {
        float detectionRadius = 1.3f; // Minimum allowed distance between defenses

        Collider[] colliders = Physics.OverlapSphere(position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Defense"))
            {
                return true; // A defense is already in this area
            }
        }

        return false; // No defenses nearby
    }
}
