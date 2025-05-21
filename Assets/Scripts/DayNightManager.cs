using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager Instance { get; private set; }

    public Material daySkybox;
    public Material nightSkybox;
    public Button startNightButton;
    public GameObject dayPanel;
    public GameObject nightPanel;

    public bool IsNightMode { get; private set; }
    public Color DayColor;
    public Color NightColor;
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
        StartDayMode(); // Day mode to start the gme
        startNightButton.onClick.AddListener(StartNightMode);
    }

    public void StartDayMode()
    {
        IsNightMode = false;
        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();
        RenderSettings.fogColor = DayColor;
        RenderSettings.fogDensity = 0.002f;

        startNightButton.gameObject.SetActive(true);

        dayPanel.SetActive(true);
        nightPanel.SetActive(false);
    }

    public void StartNightMode()
    {
        IsNightMode = true;
        RenderSettings.skybox = nightSkybox;
        DynamicGI.UpdateEnvironment();
        RenderSettings.fogColor = NightColor;
        RenderSettings.fogDensity = 0.002f;

        startNightButton.gameObject.SetActive(false);
        dayPanel.SetActive(false);
        nightPanel.SetActive(true);
      
        LevelManager.Instance.StartEnemyWave();
    }
}

