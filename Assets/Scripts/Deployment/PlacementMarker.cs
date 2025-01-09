using UnityEngine;

public class PlacementMarker : MonoBehaviour
{
    [Header("References")]
    public GameObject markerPrefab; // El objeto visual que servirá como marcador
    public LayerMask Map; // Layer del cilindro/superficie donde se pueden colocar defensas
    
    [Header("Settings")]
    public float markerHeight = 0.1f; // Altura del marcador sobre la superficie
    public Color validPlacementColor = Color.green;
    public Color invalidPlacementColor = Color.red;

    private GameObject currentMarker;
    private MeshRenderer markerRenderer;
    private Camera mainCamera;
    private bool isValidPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        
        // Crear el marcador
        currentMarker = Instantiate(markerPrefab);
        markerRenderer = currentMarker.GetComponent<MeshRenderer>();
        currentMarker.SetActive(false);
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Si el rayo golpea la superficie de colocación
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Map))
        {
            // Activar y posicionar el marcador
            currentMarker.SetActive(true);
            Vector3 markerPosition = hit.point + (Vector3.up * markerHeight);
            currentMarker.transform.position = markerPosition;
            
            // Alinear el marcador con la normal de la superficie
            currentMarker.transform.up = hit.normal;

            // Verificar si la posición es válida para colocar una defensa
            isValidPosition = CheckValidPlacement(hit.point);
            
            // Actualizar el color del marcador
            UpdateMarkerColor();
        }
        else
        {
            // Ocultar el marcador si no está sobre la superficie válida
            currentMarker.SetActive(false);
        }
    }

    private bool CheckValidPlacement(Vector3 position)
    {
        // Aquí puedes agregar tu lógica para verificar si es una posición válida
        // Por ejemplo:
        // - Comprobar si hay suficiente espacio
        // - Verificar si no hay otra defensa cerca
        // - Comprobar si el jugador tiene suficientes recursos
        
        // Por ahora, simplemente devolvemos true
        return true;
    }

    private void UpdateMarkerColor()
    {
        if (markerRenderer != null)
        {
            Material markerMaterial = markerRenderer.material;
            markerMaterial.color = isValidPosition ? validPlacementColor : invalidPlacementColor;
        }
    }

    // Método para obtener la posición actual del marcador
    public Vector3 GetCurrentPosition()
    {
        return currentMarker.transform.position;
    }

    // Método para verificar si la posición actual es válida
    public bool IsValidPosition()
    {
        return isValidPosition && currentMarker.activeSelf;
    }
}