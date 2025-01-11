using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public float mapRadius = 35f;

    private int currentLevel;
    private List<Vector2> currentAngleRanges;

    private void Start()
    {
        currentLevel = 1;
        SetAngleRangesForLevel(currentLevel);
    }

    public void AssignTargetToEnemy(EnemyAI enemyAI)
    {
        if (enemyAI != null)
        {
            Vector3 targetPoint = CalculateTargetPoint(out float targetAngle);
            Debug.Log($"Enemigo asignado al ángulo: {targetAngle}° en el nivel {currentLevel} y punto final {targetPoint}");
            enemyAI.SetTarget(targetPoint);
        }
    }

    private Vector3 CalculateTargetPoint(out float selectedAngle)
    {
        if (currentAngleRanges == null || currentAngleRanges.Count == 0)
        {
            Debug.LogError("No se pueden calcular puntos objetivo. currentAngleRanges está vacío o es null.");
            selectedAngle = 0;
            return Vector3.zero;
        }

        // Elegir un ángulo aleatorio dentro de los rangos fijos del nivel
        selectedAngle = GenerateRandomAngleFromRanges(currentAngleRanges);
        float angleInRadians = selectedAngle * Mathf.Deg2Rad;

        // Convertir el ángulo en una posición en el borde del mapa
        float x = Mathf.Cos(angleInRadians) * mapRadius;
        float z = Mathf.Sin(angleInRadians) * mapRadius;

        return new Vector3(x, 0, z);
    }

    private float GenerateRandomAngleFromRanges(List<Vector2> angleRanges)
    {
        if (angleRanges == null || angleRanges.Count == 0)
        {
            Debug.LogError("No hay rangos de ángulos disponibles.");
            return 0;
        }

        // Escoger un intervalo aleatorio
        Vector2 selectedRange = angleRanges[Random.Range(0, angleRanges.Count)];

        // Devolver un ángulo aleatorio dentro del intervalo
        return Random.Range(selectedRange.x, selectedRange.y);
    }

    private void SetAngleRangesForLevel(int level)
    {

        if (currentAngleRanges == null)
            currentAngleRanges = new List<Vector2>();
        else
            currentAngleRanges.Clear(); // Vaciar la lista existente

        // Definir los rangos de ángulos fijos por nivel
        if (level == 1)
        {
            currentAngleRanges.Add(new Vector2(0, 15));
        }
        else if (level == 2)
        {
            currentAngleRanges.Add(new Vector2(0, 30));
        }
        else if (level == 3)
        {
            currentAngleRanges.Add(new Vector2(15, 30));
            currentAngleRanges.Add(new Vector2(60, 90));
        }
        else if (level == 4)
        {
            currentAngleRanges.Add(new Vector2(0, 45));
            currentAngleRanges.Add(new Vector2(90, 120));
        }
        else
        {
            // Ejemplo para niveles más altos: Rangos múltiples o más amplios
            currentAngleRanges.Add(new Vector2(0, 90));
            currentAngleRanges.Add(new Vector2(120, 160));
        }

        // Registrar en consola los rangos definidos para el nivel actual
        Debug.Log($"Rangos de ángulos para el nivel {level}:");
        foreach (Vector2 range in currentAngleRanges)
        {
            Debug.Log($"- Rango: {range.x}° a {range.y}°");
        }

        if (currentAngleRanges.Count == 0)
        {
            Debug.LogWarning($"No se definieron rangos de ángulos para el nivel {level}. Usando rango predeterminado.");
            currentAngleRanges.Add(new Vector2(0, 360)); // Rango predeterminado
        }
    }

    public void StartNextLevel()
    {
        currentLevel++;
        SetAngleRangesForLevel(currentLevel);
    }
}
