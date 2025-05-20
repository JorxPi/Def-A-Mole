using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierIndicator : MonoBehaviour
{
    //[Header("Circle Settings")]
    //public float radius = 35f;
    //public int segments = 50;
    //public Color indicatorColor = Color.red;
    //public Color baseCircleColor = Color.gray;

    [Header("Cone Settings")]
    [Tooltip("Prefab of a cone whose pivot/base sits at its tip, pointing up its local +Z.")]
    public GameObject conePrefab;
    [Tooltip("How far inward from the rim to place the cone (in world units).")]
    public float coneInset = 1f;
    [Tooltip("Y-position to spawn cones at (e.g. -1.3)")]
    public float coneY = -1.3f;

    //private LineRenderer baseCircleRenderer;
    //private List<LineRenderer> activeAngleRenderers = new List<LineRenderer>();
    private List<GameObject> activeCones = new List<GameObject>();

    private void Awake()
    {
        //CreateBaseCircle();  // no longer needed
    }

    //private void CreateBaseCircle()
    //{
    //    ... all circle creation logic removed ...
    //}

    public void DrawIndicators(List<Vector2> angleRanges)
    {
        //ClearIndicators();  // no longer needed
        ClearCones();

        if (angleRanges == null) return;

        foreach (var range in angleRanges)
        {
            // arc drawing removed

            float midDeg = (range.x + range.y) * 0.5f;
            SpawnConeAt(midDeg);
        }
    }

    private void SpawnConeAt(float angleDeg)
    {
        if (conePrefab == null)
            return;

        float rad = angleDeg * Mathf.Deg2Rad;

        Vector3 inward = new Vector3(
            -Mathf.Cos(rad),
            0f,
            -Mathf.Sin(rad)
        ).normalized;

        Quaternion rot = Quaternion.LookRotation(inward, Vector3.up)
                         * Quaternion.Euler(0f, 180f, 0f);

        // spawn at origin with specified Y
        Vector3 spawnPos = new Vector3(0f, coneY, 0f);

        GameObject c = Instantiate(conePrefab, spawnPos, rot, transform);
        activeCones.Add(c);
    }

    //public void ClearIndicators()
    //{
    //    foreach (var lr in activeAngleRenderers)
    //        Destroy(lr.gameObject);
    //    activeAngleRenderers.Clear();
    //}

    public void ClearCones()
    {
        foreach (var c in activeCones)
            Destroy(c);
        activeCones.Clear();
    }

    //private LineRenderer CreateNewLineRenderer(Color col, float width)
    //{
    //    ... no longer needed ...
    //}
}
