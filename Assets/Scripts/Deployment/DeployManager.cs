using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployManager : MonoBehaviour
{
    public static DeployManager Instance { get; private set; }

    public GameObject map; // Assign your map GameObject here
    private bool isDeploying = false;
    private GameObject currentDefensePrefab; // Holds the prefab to deploy during deployment mode
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

        // Calculate map bounds from its collider
        if (map != null)
        {
            Collider mapCollider = map.GetComponent<Collider>();
            if (mapCollider != null)
            {
                mapBounds = mapCollider.bounds;
            }
            else
            {
                Debug.LogError("Map GameObject must have a Collider to calculate bounds.");
            }
        }
    }

    private void Update()
    {
        if (isDeploying)
        {
            HandleDeployment();
        }
    }

    public void StartDeployment(GameObject unitPrefab)
    {
        currentDefensePrefab = unitPrefab;
        isDeploying = true;
        Debug.Log("Deployment mode activated for: " + unitPrefab.name);
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
            if (hit.collider.gameObject == map && IsOnTopSurface(hit.point) /*&& !IsDefenseInLocation(hit.point)*/)
            {
                Vector3 targetPosition = hit.point;

                Transform feetTransform = currentDefensePrefab.transform.Find("Feet");

                Vector3 feetOffset = currentDefensePrefab.transform.position - feetTransform.position;
                Vector3 adjustedPosition = targetPosition + feetOffset;

                Instantiate(currentDefensePrefab, adjustedPosition, Quaternion.identity);
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
        currentDefensePrefab = null;
        Debug.Log("Deployment cancelled.");
    }

    private bool IsOnTopSurface(Vector3 position)
    {
        // Get the top surface's Y position from the map bounds
        float topSurfaceY = mapBounds.max.y;

        // Check if the target Y position matches the top surface Y
        if (Mathf.Abs(position.y - topSurfaceY) > 0.1f) // Small tolerance for float inaccuracies
        {
            return false;
        }

        // Calculate the distance from the center of the map to the target position on the XZ plane
        Vector3 mapCenter = mapBounds.center;
        Vector2 mapCenterXZ = new Vector2(mapCenter.x, mapCenter.z);
        Vector2 targetXZ = new Vector2(position.x, position.z);
        float distanceFromCenter = Vector2.Distance(mapCenterXZ, targetXZ);

        // Get the top radius from the map's bounds (assuming the cylinder's radius is half its width)
        float topRadius = mapBounds.extents.x;

        // Check if the position is within the map's top radius
        return distanceFromCenter <= topRadius;
    }

    //No funciona, he de mirar

    /*private bool IsDefenseInLocation(Vector3 position)
    {
        if (currentDefensePrefab == null)
        {
            Debug.LogWarning("No defense prefab assigned for deployment.");
            return false;
        }

        // Get the radius of the current defense prefab based on its collider
        Collider defenseCollider = currentDefensePrefab.GetComponent<Collider>();
        if (defenseCollider == null)
        {
            Debug.LogWarning("Defense prefab does not have a collider.");
            return false;
        }

        float defenseRadius = defenseCollider.bounds.extents.magnitude;
        float detectionRadius = defenseRadius * 2; // Set minimum detection distance as twice the defense radius

        Collider[] colliders = Physics.OverlapSphere(position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Defense")) // Tag defenses with "Defense"
            {
                // Calculate the distance from the target position to the detected defense
                float distanceToDefense = Vector3.Distance(position, collider.transform.position);

                // Calculate an effective distance to prevent overlap, including an offset
                float existingDefenseRadius = collider.bounds.extents.magnitude;
                float requiredClearance = defenseRadius + existingDefenseRadius; // Minimum distance required to avoid overlap

                if (distanceToDefense < requiredClearance)
                {
                    return true; // There's already a defense within the required clearance distance
                }
            }
        }

        return false;
    }*/
}
