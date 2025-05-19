using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Map Bounds")]
    public Collider mapBounds;

    [Header("Rotation Controls")]
    public float horizontalSpeed = 40f;
    public float verticalSpeed = 20f;
    public float dragSpeed = 300f;

    [Header("Zoom Controls")]
    public float minZoomDistance = 25f;
    public float maxZoomDistance = 55f;
    public float zoomSpeed = 10f;
    public float zoomLerpSpeed = 10f;

    [Header("Pitch (Tilt) Limits")]
    [Tooltip("20° means 20° above the horizon; 90° is straight down")]
    public float minPitchAngle = 20f;
    public float maxPitchAngle = 90f;

    [Tooltip("Tiny offset so we never hit exactly maxPitchAngle")]
    public float pitchEpsilon = 0.01f;

    // Internals
    private Vector3 mapCenter;
    private float currentYaw;       // degrees
    private float currentPitch;     // degrees
    private float currentDistance;  // radius
    private float targetDistance;   // for smooth zoom

    void Start()
    {
        CalculateMapCenter();

        // 1) Slam the camera out to the farthest allowed distance
        Vector3 initDir = (transform.position - mapCenter).normalized;
        if (initDir == Vector3.zero) initDir = Vector3.forward;
        transform.position = mapCenter + initDir * maxZoomDistance;

        // 2) Initialize our spherical coordinates
        Vector3 offset = transform.position - mapCenter;
        currentDistance = maxZoomDistance;
        targetDistance = maxZoomDistance;

        // avoid exact singularity at maxPitchAngle
        float safeMaxPitch = maxPitchAngle - pitchEpsilon;
        currentPitch = Mathf.Clamp(
            Mathf.Asin(offset.y / currentDistance) * Mathf.Rad2Deg,
            minPitchAngle,
            safeMaxPitch
        );
        currentYaw = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;
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
        if (Input.GetKey(KeyCode.A)) currentYaw -= horizontalSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) currentYaw += horizontalSpeed * Time.deltaTime;
        if (Input.GetMouseButton(1))
            currentYaw += Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;

        currentYaw %= 360f;
    }

    void HandleVerticalRotation()
    {
        float safeMaxPitch = maxPitchAngle - pitchEpsilon;
        if (Input.GetKey(KeyCode.W))
            currentPitch = Mathf.Clamp(currentPitch + verticalSpeed * Time.deltaTime, minPitchAngle, safeMaxPitch);
        if (Input.GetKey(KeyCode.S))
            currentPitch = Mathf.Clamp(currentPitch - verticalSpeed * Time.deltaTime, minPitchAngle, safeMaxPitch);

        if (Input.GetMouseButton(1))
            currentPitch = Mathf.Clamp(
                currentPitch - Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime,
                minPitchAngle,
                safeMaxPitch
            );
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
            targetDistance = Mathf.Clamp(targetDistance - scroll * zoomSpeed, minZoomDistance, maxZoomDistance);

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomLerpSpeed);
    }

    void UpdateCameraPosition()
    {
        float radPitch = currentPitch * Mathf.Deg2Rad;
        float radYaw = currentYaw * Mathf.Deg2Rad;

        float y = mapCenter.y + Mathf.Sin(radPitch) * currentDistance;
        float r = Mathf.Cos(radPitch) * currentDistance;
        float x = mapCenter.x + r * Mathf.Cos(radYaw);
        float z = mapCenter.z + r * Mathf.Sin(radYaw);

        transform.position = new Vector3(x, y, z);

        transform.LookAt(mapCenter, Vector3.up);
    }

    void CalculateMapCenter()
    {
        mapCenter = (mapBounds != null)
            ? mapBounds.transform.position
            : Vector3.zero;
    }
}
