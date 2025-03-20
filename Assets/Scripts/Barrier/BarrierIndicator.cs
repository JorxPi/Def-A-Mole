using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierIndicator : MonoBehaviour
{
    public float radius = 35f; // Match mapRadius from LevelManager
    public int segments = 50; // Number of points in the arc
    public Color indicatorColor = Color.red; // Color for active angles
    public Color baseCircleColor = Color.gray; // Color for full circle

    private List<LineRenderer> activeAngleRenderers = new List<LineRenderer>();
    private LineRenderer baseCircleRenderer; // Full circle renderer

    private void Awake()
    {
        CreateBaseCircle(); // Creates the full circle initially
    }

    private void CreateBaseCircle()
    {
        baseCircleRenderer = CreateNewLineRenderer(baseCircleColor, 0.3f);

        List<Vector3> circlePoints = new List<Vector3>();
        for (int i = 0; i <= segments; i++)
        {
            float angle = (i / (float)segments) * 360f * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            circlePoints.Add(new Vector3(x, 0.1f, z));
        }

        baseCircleRenderer.positionCount = circlePoints.Count;
        baseCircleRenderer.SetPositions(circlePoints.ToArray());

        baseCircleRenderer.loop = true;
    }

    public void DrawIndicators(List<Vector2> angleRanges)
    {
        if (angleRanges == null || angleRanges.Count == 0)
        {
            Debug.LogWarning("No angle ranges provided. Skipping visualization.");
            return;
        }

        ClearIndicators(); // Remove old indicators before drawing

        foreach (Vector2 range in angleRanges)
        {
            Debug.Log($"Drawing indicator for angle range: {range.x}° to {range.y}°");

            LineRenderer newRenderer = CreateNewLineRenderer(indicatorColor, 1.0f);
            activeAngleRenderers.Add(newRenderer);

            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                float angle = Mathf.Lerp(range.x, range.y, t) * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                points.Add(new Vector3(x, 0.1f, z));
            }

            newRenderer.positionCount = points.Count;
            newRenderer.SetPositions(points.ToArray());
        }

        Debug.Log($"Total indicators drawn: {activeAngleRenderers.Count}");
    }

    private LineRenderer CreateNewLineRenderer(Color color, float width)
    {
        GameObject lineObj = new GameObject("IndicatorLine");
        lineObj.transform.SetParent(transform);

        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.useWorldSpace = true;
        return lineRenderer;
    }

    public void ClearIndicators()
    {
        foreach (LineRenderer lr in activeAngleRenderers)
        {
            Destroy(lr.gameObject);
        }
        activeAngleRenderers.Clear();
    }
}
