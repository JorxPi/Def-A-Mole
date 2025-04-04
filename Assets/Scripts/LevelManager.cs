using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float mapRadius;
    public TextMeshProUGUI levelText;

    private int currentLevel;
    public List<Vector2> currentAngleRanges;
    public int enemiesAlive;

    public List<DefenseData> unlockedDefenses = new List<DefenseData>();
    public List<DefenseData> allDefenses;

    private int totalEnemiesThisWave;

    private BarrierIndicator barrierIndicator;

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentLevel = 1;
        unlockedDefenses = new List<DefenseData>();

        barrierIndicator = FindObjectOfType<BarrierIndicator>();

        SetAngleRangesForLevel(currentLevel);
        UnlockDefensesForLevel(currentLevel);
        UpdateLevelText();
        UpdateDefenseButtons();
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

    /*private void SetAngleRangesForLevel(int level)
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

        if (barrierIndicator != null)
        {
            Debug.Log("Calling DrawIndicators()...");
            barrierIndicator.DrawIndicators(currentAngleRanges);
        }
        else
        {
            Debug.LogError("BarrierIndicator is NULL in LevelManager!");
        }
    }*/

    private void SetAngleRangesForLevel(int level)
    {
        if (currentAngleRanges == null)
            currentAngleRanges = new List<Vector2>();
        else
            currentAngleRanges.Clear(); // Clear previous ranges

        int anglesToGenerate = Mathf.Min(level, 24); // Max 24 sets (360° / 15°)

        HashSet<int> usedAngles = new HashSet<int>(); // To track already used angles

        for (int i = 0; i < anglesToGenerate; i++)
        {
            int randomAngle;
            do
            {
                randomAngle = Random.Range(0, 24) * 15; // Only multiples of 15
            } while (usedAngles.Contains(randomAngle)); // Ensure no duplicates

            usedAngles.Add(randomAngle);
            currentAngleRanges.Add(new Vector2(randomAngle, randomAngle + 15));
        }

        Debug.Log($"Level {level} Angle Ranges: {string.Join(" | ", currentAngleRanges)}");

        if (barrierIndicator != null)
        {
            barrierIndicator.DrawIndicators(currentAngleRanges); // Update visualization
        }
        else
        {
            Debug.LogError("BarrierIndicator is NULL in LevelManager!");
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
        Debug.Log($"Un enemigo ha sido destruido. Enemigos restantes: {enemiesAlive} / {totalEnemiesThisWave}");

        CheckForNextLevel();
    }

    public void StartNextLevel()
    {
        currentLevel++;
        SetAngleRangesForLevel(currentLevel);
        UpdateLevelText();

        UnlockDefensesForLevel(currentLevel);
        UpdateDefenseButtons();

        if (barrierIndicator != null)
        {
            barrierIndicator.ClearIndicators();
            barrierIndicator.DrawIndicators(currentAngleRanges);
        }

        DayNightManager.Instance.StartDayMode();
    }

    public void StartEnemyWave()
    {
        EnemySpawner1 enemySpawner = FindObjectOfType<EnemySpawner1>();

        totalEnemiesThisWave = enemySpawner.GetEnemiesToSpawn(currentLevel);
        enemiesAlive = totalEnemiesThisWave;

        enemySpawner.StartLevel(currentLevel);

        Debug.Log($"[Level {currentLevel}] Iniciando oleada nocturna con {totalEnemiesThisWave} enemigos.");
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

    private void UnlockDefensesForLevel(int level)
    {
        switch (level)
        {
            case 1:
                AddDefenseToUnlock("Ant");
                AddDefenseToUnlock("Worm");
                AddDefenseToUnlock("Snail");
                AddDefenseToUnlock("Beatle");
                AddDefenseToUnlock("Beehive");
                break;
            case 3:
                //AddDefenseToUnlock("Snail");
                break;
            case 5:
                //AddDefenseToUnlock("Beatle");
                break;
            case 7:
                //AddDefenseToUnlock("Beehive");
                break;
        }

        UpdateDefenseButtons(); // Refresh UI
    }

    private void AddDefenseToUnlock(string defenseName)
    {
        DefenseData defenseToUnlock = allDefenses.Find(d => d.defenseName == defenseName);
        if (defenseToUnlock != null && !unlockedDefenses.Contains(defenseToUnlock))
        {
            unlockedDefenses.Add(defenseToUnlock);
            Debug.Log($"Unlocked {defenseName} at level {currentLevel}");
        }
    }

    public void UpdateDefenseButtons()
    {
        foreach (DeployButton button in FindObjectsOfType<DeployButton>())
        {
            bool isUnlocked = unlockedDefenses.Contains(button.defenseData);
            button.GetComponent<Button>().interactable = isUnlocked; // Keep visible but disable interaction
        }
    }
}
