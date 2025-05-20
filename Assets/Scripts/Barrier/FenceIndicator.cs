using UnityEngine;
using System.Collections.Generic;

public class FenceIndicator : MonoBehaviour
{
    [Header("Fence Setup")]
    [Tooltip("The 15° curved fence prefab with FenceSegment on it.")]
    public GameObject fenceSegmentPrefab;
    [Tooltip("Radius at which to place the fence (world units).")]
    public float radius = 50f;
    [Tooltip("Degrees covered by each prefab instance (must match your model).")]
    public int segmentAngle = 15;

    // Internally stored segments:
    private List<FenceSegment> _segments = new List<FenceSegment>();

    private void Awake()
    {
        BuildFenceRing();
    }

    /// <summary>
    /// Instantiates the ring of fence segments.
    /// </summary>
    private void BuildFenceRing()
    {
        if (fenceSegmentPrefab == null)
        {
            Debug.LogError("FenceIndicator: fenceSegmentPrefab not assigned!");
            return;
        }

        int count = 360 / segmentAngle;
        for (int i = 0; i < count; i++)
        {
            float angDeg = i * segmentAngle;
            float angRad = angDeg * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                Mathf.Cos(angRad) * radius,
                transform.position.y,
                Mathf.Sin(angRad) * radius
            );
            Quaternion rot = Quaternion.Euler(0, angDeg + 90f, 0);

            GameObject go = Instantiate(fenceSegmentPrefab, pos, rot, transform);
            FenceSegment fs = go.GetComponent<FenceSegment>();
            if (fs != null)
                _segments.Add(fs);
            else
                Debug.LogWarning("FenceIndicator: prefab missing FenceSegment script!");
        }
    }

    /// <summary>
    /// Highlights any segments whose base-angle lies within any of the given ranges.
    /// Ranges are Vector2(startDeg, endDeg). Can wrap past 360 - 0.
    /// </summary>
    public void DrawIndicators(List<Vector2> angleRanges)
    {
        // 1) reset all
        foreach (var seg in _segments)
            seg.SetHighlight(false);

        // 2) highlight matching ones
        for (int i = 0; i < _segments.Count; i++)
        {
            float baseAngle = i * segmentAngle;
            foreach (var range in angleRanges)
            {
                if (IsAngleBetween(baseAngle, range.x, range.y))
                {
                    _segments[i].SetHighlight(true);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Utility to check if angle lies in [start,end], including wraparound.
    /// </summary>
    private bool IsAngleBetween(float angle, float start, float end)
    {
        angle = (angle + 360f) % 360f;
        start = (start + 360f) % 360f;
        end = (end + 360f) % 360f;

        if (start <= end)
            return angle >= start && angle <= end;
        else
            return angle >= start || angle <= end;
    }

    /// <summary>
    /// Clears all highlights.
    /// </summary>
    public void ClearIndicators()
    {
        foreach (var seg in _segments)
            seg.SetHighlight(false);
    }
}

