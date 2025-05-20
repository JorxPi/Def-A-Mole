using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class Highlightable : MonoBehaviour
{
    [Tooltip("Color to use when this object is selected.")]
    public Color highlightColor = Color.yellow;

    // Maps each live Renderer to its original color
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    // The last-highlighted instance
    private static Highlightable currentSelection;

    private void Awake()
    {
        CacheOriginalColors();
    }

    /// <summary>
    /// Scans all child Renderers and records their current material colors.
    /// Clears out any entries for destroyed renderers first.
    /// </summary>
    public void CacheOriginalColors()
    {
        // Remove any destroyed keys
        foreach (var dead in originalColors.Keys.Where(r => r == null).ToList())
            originalColors.Remove(dead);

        // Add any new Renderers
        foreach (var rend in GetComponentsInChildren<Renderer>())
            if (!originalColors.ContainsKey(rend))
                originalColors[rend] = rend.material.color;
    }

    /// <summary>
    /// Select this object: deselect the previous, then apply highlight color.
    /// </summary>
    public void Select()
    {
        // Re-cache in case you just swapped meshes/materials in an upgrade
        CacheOriginalColors();

        if (currentSelection != null && currentSelection != this)
            currentSelection.Deselect();

        // Paint every still-alive renderer
        foreach (var kv in originalColors)
            if (kv.Key != null)
                kv.Key.material.color = highlightColor;

        currentSelection = this;
    }

    /// <summary>
    /// Return all renderers to their recorded original colors.
    /// </summary>
    public void Deselect()
    {
        // Paint back only those that still exist
        foreach (var kv in originalColors)
            if (kv.Key != null)
                kv.Key.material.color = kv.Value;

        if (currentSelection == this)
            currentSelection = null;
    }

    /// <summary>
    /// Static helper to clear any current selection.
    /// </summary>
    public static void DeselectCurrent()
    {
        currentSelection?.Deselect();
    }
}
