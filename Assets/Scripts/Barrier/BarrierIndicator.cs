using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BarrierIndicator : MonoBehaviour
{
    public float radius = 35f; // Match mapRadius from LevelManager
    public int segments = 30; // Number of points in the arc
    public Color indicatorColor = Color.red; // Color of the lines
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = indicatorColor;
        lineRenderer.endColor = indicatorColor;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 0; // Start empty
    }

    public void DrawIndicators(List<Vector2> angleRanges)
    {
        if (angleRanges == null || angleRanges.Count == 0)
        {
            Debug.LogWarning("No angle ranges provided. Skipping visualization.");
            return;
        }

        List<Vector3> points = new List<Vector3>();

        foreach (Vector2 range in angleRanges)
        {
            Debug.Log($"Drawing indicator for angle range: {range.x}° to {range.y}°");

            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(range.x, range.y, (float)i / segments) * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                points.Add(new Vector3(x, 0.1f, z)); // Slightly above ground
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        Debug.Log($"Indicator drawn with {points.Count} points.");
    }

    public void ClearIndicators()
    {
        lineRenderer.positionCount = 0;
    }
}
