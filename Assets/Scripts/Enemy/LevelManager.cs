using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public float mapRadius = 35f;
    public TextMeshProUGUI levelText;

    private int currentLevel;
    private List<Vector2> currentAngleRanges;
    public int enemiesAlive;
    private void Start()
    {
        currentLevel = 1;
        SetAngleRangesForLevel(currentLevel);
        UpdateLevelText();
    }

    public void AssignTargetToEnemy(EnemyAI enemyAI)
    {

        if (enemyAI != null)
        {
            Vector3 targetPoint = CalculateTargetPoint(out float targetAngle);
            Debug.Log($"Enemigo asignado al ángulo: {targetAngle}° en el nivel {currentLevel} y punto final {targetPoint}");
            enemyAI.SetTarget(targetPoint);
            //enemies.Add(enemy);
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

    
    private void CheckForNextLevel()
    {
        if (enemiesAlive == 0)
        {
            // Todos los enemigos han muerto, pasar al siguiente nivel
            StartNextLevel();
        }
    }
    

    public void EnemyDied()
    {
        enemiesAlive--;
        Debug.Log($"Un enemigo ha sido destruido. ({enemiesAlive} enemies)");
        CheckForNextLevel();
    }

    public void StartNextLevel()
    {
        currentLevel++;
        SetAngleRangesForLevel(currentLevel);
        UpdateLevelText();
        EnemySpawner1 enemySpawner = FindObjectOfType<EnemySpawner1>();
        enemySpawner.StartLevel(currentLevel);
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = $"Round: {currentLevel}";

            // Only animate from level 2 onwards
            if (currentLevel > 1)
            {
                AnimateLevelText();
            }
        }
    }

    private void AnimateLevelText()
    {
        if (levelText == null) return;
        StartCoroutine(LevelUpAnimation());
    }

    private IEnumerator LevelUpAnimation()
    {
        float duration = 3f; // Total animation time
        float elapsedTime = 0f;
        Vector3 originalScale = levelText.transform.localScale;
        Vector3 maxScale = originalScale * 1.3f;

        while (elapsedTime < duration)
        {
            // Smooth scale transition using PingPong effect
            float scaleFactor = Mathf.Lerp(1f, 1.3f, Mathf.PingPong(elapsedTime * 2, 1));
            levelText.transform.localScale = originalScale * scaleFactor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure scale resets to normal after the animation
        levelText.transform.localScale = originalScale;
    }
}
