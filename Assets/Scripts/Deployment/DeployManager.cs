using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployManager : MonoBehaviour
{
    public static DeployManager Instance { get; private set; }

    public GameObject map; 
    private bool isDeploying = false;
    private DefenseData currentDefenseData;
    private Bounds mapBounds;

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

        Collider mapCollider = map.GetComponent<Collider>();
        mapBounds = mapCollider.bounds;

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

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Ensure we clicked on the map and validate deployment conditions
            if (hit.collider.gameObject == map && !IsDefenseInLocation(hit.point))
            {
                // Use the exact hit point to place the unit at floor level
                Vector3 targetPosition = hit.point;

                // Adjust for the feet offset (if necessary)
                Transform feetTransform = currentDefenseData.prefab.transform.Find("Feet");
                Vector3 feetOffset = Vector3.zero;

                if (feetTransform != null)
                {
                    feetOffset = currentDefenseData.prefab.transform.position - feetTransform.position;
                }

                // Ensure the unit spawns at the correct floor level
                Vector3 adjustedPosition = new Vector3(targetPosition.x, hit.point.y, targetPosition.z) + feetOffset;

                // Instantiate the defense unit
                Instantiate(currentDefenseData.prefab, adjustedPosition, Quaternion.identity);

                // Deduct the cost and finalize deployment
                CurrencyManager.Instance.SpendGold(currentDefenseData.cost);
                Debug.Log("Unit deployed at: " + adjustedPosition);
                isDeploying = false;
            }
            else
            {
                CancelDeployment();
            }
        }
    }

    private void CancelDeployment()
    {
        isDeploying = false;
        currentDefenseData = null;
        Debug.Log("Deployment cancelled.");
    }

    private bool IsOnTopSurface(Vector3 position)
    {
        Vector3 mapCenter = mapBounds.center;
        Vector2 mapCenterXZ = new Vector2(mapCenter.x, mapCenter.z);
        Vector2 targetXZ = new Vector2(position.x, position.z);

        float distanceFromCenter = Vector2.Distance(mapCenterXZ, targetXZ);

        float topRadius = mapBounds.extents.x;

        return distanceFromCenter <= topRadius; // Only check within bounds, skip Y-check
    }

    private bool IsDefenseInLocation(Vector3 position)
    {
        Collider defenseCollider = currentDefenseData.prefab.GetComponent<Collider>();
        float distanceBetweenDefenses = 1.3f;

        // Radius of the new defense to be deployed
        float defenseRadius = defenseCollider.bounds.extents.magnitude;

        float detectionRadius = defenseRadius + distanceBetweenDefenses;

        Collider[] colliders = Physics.OverlapSphere(position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Defense"))
            {
                float distanceToDefense = Vector3.Distance(position, collider.transform.position);

                float existingDefenseRadius = collider.bounds.extents.magnitude;

                float requiredClearance = defenseRadius + existingDefenseRadius + distanceBetweenDefenses;

                if (distanceToDefense < requiredClearance)
                {
                    return true; 
                }
            }
        }

        return false; 
    }

}
