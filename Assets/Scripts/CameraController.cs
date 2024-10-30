using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Collider mapBounds;
    public float horizontalSpeed = 30f;
    public float verticalSpeed = 10f;
    public float minHeight = 6f;
    public float maxHeight = 25f;
    public float radiusOffset = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 2f; 


    private float currentAngle = 0f;
    private float currentHeight;
    private Vector3 mapCenter;
    private float halfWidth;
    private float halfLength;
    private float zoomDistance;

    void Start()
    {
        CalculateMapCenterAndRadius();
        currentHeight = minHeight;
        zoomDistance = radiusOffset;
    }

    void Update()
    {
        // Calculem angle horitzontal
        HandleHorizontalRotation();
        // Calculem la posició vertical
        HandleVerticalRotation();

        HandleZoom();

        // Actualitzem posició de la camera
        UpdateCameraPosition();
    }

    void HandleHorizontalRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            currentAngle -= horizontalSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            currentAngle += horizontalSpeed * Time.deltaTime;
        }

        currentAngle %= 360f;
    }

    void HandleVerticalRotation()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentHeight = Mathf.Clamp(currentHeight + verticalSpeed * Time.deltaTime, minHeight, maxHeight);
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentHeight = Mathf.Clamp(currentHeight - verticalSpeed * Time.deltaTime, minHeight, maxHeight);
        }
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            zoomDistance = Mathf.Clamp(zoomDistance - scrollInput * zoomSpeed, minZoom, radiusOffset);
        }
    }

    void UpdateCameraPosition()
    {
        float x = mapCenter.x + (halfWidth + zoomDistance) * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float z = mapCenter.z + (halfLength + zoomDistance) * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector3 topPosition = new Vector3(mapCenter.x, maxHeight, mapCenter.z); 
        Vector3 currentCirclePosition = new Vector3(x, currentHeight, z);       

        float heightRatio = Mathf.Clamp01((currentHeight - minHeight) / (maxHeight - minHeight) * 0.999f);
        transform.position = Vector3.Lerp(currentCirclePosition, topPosition, heightRatio);

        transform.LookAt(new Vector3(mapCenter.x, 0, mapCenter.z));
    }

    void CalculateMapCenterAndRadius()
    {
        mapCenter = mapBounds.transform.position;

        halfWidth = mapBounds.transform.localScale.x / 2;
        halfLength = mapBounds.transform.localScale.z / 2;
    }
}
