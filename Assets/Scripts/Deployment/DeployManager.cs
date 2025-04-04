using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DeployManager : MonoBehaviour
{
    public static DeployManager Instance { get; private set; }

    public GameObject map; 
    public LayerMask mapLayer; 
    public SphereCollider restrictedZone;
    public float mapRadius;


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
        int adjustedCost = DayNightManager.Instance.IsNightMode ? Mathf.CeilToInt(defenseData.cost * 1.3f) : defenseData.cost;

        if (CurrencyManager.Instance.HasEnoughGold(adjustedCost))
        {
            currentDefenseData = defenseData;
            isDeploying = true;

            if (currentDefenseData.prefab != null)
            {
                previewInstance = Instantiate(currentDefenseData.prefab);

                DisableScripts(previewInstance);

                SetCollidersEnabled(previewInstance, false);
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

            Transform previewBase = previewInstance.transform.Find("Base");
            if (previewBase == null)
            {
                Debug.LogError("Preview Base not found!");
                return;
            }

            Debug.Log(!IsDefenseInLocation());
            Debug.Log(IsInsideMap(hit.point));
            Debug.Log(!IsInsideRestrictedZone(hit.point));


            bool isValidPlacement = !IsDefenseInLocation() && IsInsideMap(hit.point) && !IsInsideRestrictedZone(hit.point);
            UpdatePreviewColor(isValidPlacement);
        }
    }

    private void PlaceDefense()
    {
        if (previewInstance == null) return;

        int adjustedCost = DayNightManager.Instance.IsNightMode ? Mathf.CeilToInt(currentDefenseData.cost * 1.3f) : currentDefenseData.cost;

        if (!IsDefenseInLocation() && IsInsideMap(previewInstance.transform.position) && !IsInsideRestrictedZone(previewInstance.transform.position))
        {
            GameObject placedDefense = Instantiate(currentDefenseData.prefab, previewInstance.transform.position, Quaternion.identity);
            SetCollidersEnabled(placedDefense, true);

            CurrencyManager.Instance.SpendGold(adjustedCost);
            CleanupPreview();
        }
        else
        {
            Debug.Log("Placement blocked: base is overlapping another defense.");
            CancelDeployment();
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

    private bool IsDefenseInLocation()
    {
        if (previewInstance == null) return false;

        Transform previewBase = previewInstance.transform.Find("Base");
        Collider previewBaseCollider = previewBase.GetComponent<Collider>();

        float radius = previewBaseCollider.bounds.extents.x;
        Vector3 center = previewBaseCollider.bounds.center;

        Collider[] colliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("BaseLayer"));

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != previewBase.gameObject)
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

        // Calculate distance from center (only XZ plane)
        float distanceFromCenter = Vector3.Distance(new Vector3(position.x, mapCenter.y, position.z), new Vector3(mapCenter.x, mapCenter.y, mapCenter.z));

        // Check if within map radius and on the top surface
        bool isWithinRadius = distanceFromCenter <= mapRadius;
        bool isOnTopSurface = position.y >= mapCenter.y; // Ensure placement is on top

        return isWithinRadius && isOnTopSurface;
    }

    private bool IsInsideRestrictedZone(Vector3 position)
    {
        if (restrictedZone == null) return false;

        // Obtener el punto más cercano en el SphereCollider
        Vector3 closestPoint = restrictedZone.ClosestPoint(position);

        // Si la posición está dentro del radio del collider, significa que está en la zona restringida
        return Vector3.Distance(closestPoint, position) < 0.01f;
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

    private void SetCollidersEnabled(GameObject obj, bool enabled)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("BaseLayer"))
            {
                collider.isTrigger = !enabled; // Make it a trigger during preview
            }
            else
            {
                collider.enabled = enabled; // Fully enable for non-base colliders
            }
        }

        NavMeshObstacle[] navMeshObstacles = obj.GetComponentsInChildren<NavMeshObstacle>();
        foreach (NavMeshObstacle obstacle in navMeshObstacles)
        {
            obstacle.enabled = enabled;
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

