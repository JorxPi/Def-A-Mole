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
    private GameObject previewInstance;
    private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();

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
            UpdatePreviewPosition();
            HandleDeployment();
        }
    }

    public void StartDeployment(DefenseData defenseData)
    {
        if (CurrencyManager.Instance.HasEnoughGold(defenseData.cost))
        {
            currentDefenseData = defenseData;
            isDeploying = true;

            // Instantiate the preview from the prefab inside defenseData
            if (currentDefenseData.prefab != null)
            {
                previewInstance = Instantiate(currentDefenseData.prefab);

                // Disable attack scripts and other behaviors
                DisableScripts(previewInstance);

                // Disable physics interactions for the preview
                DisableCollisions(previewInstance);
            }
        }
        else
        {
            Debug.Log("Not enough gold to deploy this defense!");
        }
    }

    private void HandleDeployment()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to cancel
        {
            CancelDeployment();
        }
        else if (Input.GetMouseButtonDown(0)) // Left-click to place
        {
            PlaceDefense();
        }
    }

    private void UpdatePreviewPosition()
    {
        if (previewInstance == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mapLayer))
        {
            Vector3 feetOffset = GetFeetOffset();
            previewInstance.transform.position = hit.point + feetOffset;

            // Check if placement is valid (not on another defense AND inside the map boundaries)
            bool isValidPlacement = !IsDefenseInLocation(hit.point) && IsInsideMap(hit.point);

            // Update preview color based on placement validity
            UpdatePreviewColor(isValidPlacement);
        }
    }

    private void PlaceDefense()
    {
        if (previewInstance == null) return;

        Vector3 position = previewInstance.transform.position;

        if (!IsDefenseInLocation(position) && IsInsideMap(position))
        {
            Instantiate(currentDefenseData.prefab, position, Quaternion.identity);
            CurrencyManager.Instance.SpendGold(currentDefenseData.cost);
            CleanupPreview();
        }
        else
        {
            Debug.Log("Placement blocked (either too close to another defense or outside the map).");
            CancelDeployment(); // Cancel if placement is invalid
        }
    }

    private void CancelDeployment()
    {
        isDeploying = false;
        currentDefenseData = null;
        CleanupPreview();
    }

    private void CleanupPreview()
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
    }

    private bool IsDefenseInLocation(Vector3 position)
    {
        float detectionRadius = 1.3f;
        Collider[] colliders = Physics.OverlapSphere(position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Defense"))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsInsideMap(Vector3 position)
    {
        // Get map's position and scale
        Vector3 mapCenter = map.transform.position;
        float mapRadius = map.transform.localScale.x * 0.5f; // Assuming X scale is the diameter

        // Calculate distance from center (only XZ plane)
        float distanceFromCenter = Vector3.Distance(new Vector3(position.x, mapCenter.y, position.z), new Vector3(mapCenter.x, mapCenter.y, mapCenter.z));

        // Check if within map radius and on the top surface
        bool isWithinRadius = distanceFromCenter <= mapRadius;
        bool isOnTopSurface = position.y >= mapCenter.y; // Ensure placement is on top

        return isWithinRadius && isOnTopSurface;
    }

    private void UpdatePreviewColor(bool isValid)
    {
        if (previewInstance == null) return;

        // Get all MeshRenderers in the preview instance
        MeshRenderer[] renderers = previewInstance.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0) return;

        // Set color to all renderers
        Color targetColor = isValid ? Color.white : new Color(1f, 0.5f, 0.5f, 1f); // Light red if invalid
        foreach (MeshRenderer renderer in renderers)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor("_Color", targetColor);
            renderer.SetPropertyBlock(block);
        }
    }

    private Vector3 GetFeetOffset()
    {
        if (currentDefenseData != null && currentDefenseData.prefab != null)
        {
            Transform feetTransform = currentDefenseData.prefab.transform.Find("Feet");
            if (feetTransform != null)
            {
                return currentDefenseData.prefab.transform.position - feetTransform.position;
            }
        }
        return Vector3.zero;
    }

    private void DisableCollisions(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false; // Disable collisions for preview
        }
    }

    private void DisableScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            // Skip disabling RotateTowardsEnemy to keep rotation working
            if (script is RotateTowardsEnemy) continue;

            if (script != this) // Don't disable DeployManager itself
            {
                script.enabled = false;
                disabledScripts.Add(script);
            }
        }
    }
}

