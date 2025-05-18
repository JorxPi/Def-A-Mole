using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Highlightable : MonoBehaviour
{
    // Color que usarás para resaltar (asigna en el Inspector)
    public Color highlightColor = Color.yellow;

    // Guarda los colores originales de cada renderer
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    // Último Highlightable seleccionado
    private static Highlightable currentSelection;

    private void Awake()
    {
        // Capturamos los colores originales
        foreach (var r in GetComponentsInChildren<Renderer>())
            originalColors[r] = r.material.color;
    }

    /// <summary>
    /// Resalta esta defensa, y des-resalta la anterior si existe
    /// </summary>
    public void Select()
    {
        // Si ya hay otra selección, la deseleccionamos
        if (currentSelection != null && currentSelection != this)
            currentSelection.Deselect();

        // Pintamos todos los renderers con el color de highlight
        foreach (var kv in originalColors)
            kv.Key.material.color = highlightColor;

        currentSelection = this;
    }

    /// <summary>
    /// Vuelve al color original
    /// </summary>
    public void Deselect()
    {
        foreach (var kv in originalColors)
            kv.Key.material.color = kv.Value;

        if (currentSelection == this)
            currentSelection = null;
    }

    /// <summary>
    /// Deselecciona la defensa actualmente resaltada (si la hay)
    /// </summary>
    public static void DeselectCurrent()
    {
        if (currentSelection != null)
            currentSelection.Deselect();
    }
}

