using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleVisualizer : MonoBehaviour
{
    public float radius = 35f;
    public int segments = 20;
    public LineRenderer lineRenderer;
    private LevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = (segments + 1) * levelManager.currentAngleRanges.Count;
        lineRenderer.loop = false;
        DrawAngles();
    }

    public void DrawAngles()
    {
        if (levelManager == null) return;

        int index = 0;
        foreach (Vector2 range in levelManager.currentAngleRanges)
        {
            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(range.x, range.y, i / (float)segments);
                Vector3 point = GetPointFromAngle(angle);
                lineRenderer.SetPosition(index, point);
                index++;
            }
        }
    }

    private Vector3 GetPointFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);
    }
}
