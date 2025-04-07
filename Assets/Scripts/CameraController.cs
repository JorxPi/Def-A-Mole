using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Collider mapBounds;

    [Header("Orbit Controls")]
    public float orbitDistance = 55f;
    public float minZoomDistance = 25f;
    public float maxZoomDistance = 55f;
    public float zoomSpeed = 10f;

    [Header("Camera Rotation")]
    public float horizontalSpeed = 30f;
    public float verticalSpeed = 10f;
    public float minHeight = 6f;
    public float maxHeight = 25f;

    private float currentAngle = 0f;
    private float currentHeight;
    private Vector3 mapCenter;
    private float targetOrbitDistance;
    public float zoomLerpSpeed = 10f; 

    void Start()
    {
        CalculateMapCenter();
        currentHeight = minHeight;
        targetOrbitDistance = orbitDistance;
    }

    void Update()
    {
        HandleHorizontalRotation();
        HandleVerticalRotation();
        HandleZoom();
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
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetOrbitDistance -= scroll * zoomSpeed;
            targetOrbitDistance = Mathf.Clamp(targetOrbitDistance, minZoomDistance, maxZoomDistance);
        }
        orbitDistance = Mathf.Lerp(orbitDistance, targetOrbitDistance, Time.deltaTime * zoomLerpSpeed);

    }

    void UpdateCameraPosition()
    {
        float x = mapCenter.x + orbitDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float z = mapCenter.z + orbitDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector3 topPosition = new Vector3(mapCenter.x, maxHeight, mapCenter.z);
        Vector3 currentCirclePosition = new Vector3(x, currentHeight, z);

        float heightRatio = Mathf.Clamp01((currentHeight - minHeight) / (maxHeight - minHeight) * 0.999f);
        transform.position = Vector3.Lerp(currentCirclePosition, topPosition, heightRatio);

        transform.LookAt(new Vector3(mapCenter.x, 0, mapCenter.z));
    }

    void CalculateMapCenter()
    {
        mapCenter = mapBounds.transform.position;
    }
}
