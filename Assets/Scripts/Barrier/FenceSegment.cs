using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FenceSegment : MonoBehaviour
{
    [Tooltip("Default material for this segment.")]
    public Material normalMat;
    [Tooltip("Material to use when this segment is highlighted.")]
    public Material highlightMat;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = normalMat;
    }

    /// <summary>
    /// Toggles this segment between normal and highlighted state.
    /// </summary>
    public void SetHighlight(bool on)
    {
        _renderer.material = on ? highlightMat : normalMat;
    }
}