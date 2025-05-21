using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button quitButton;
    public Text pauseText;
    public List<Button> uiButtonsToDisable;
    public Button retryButton;

    private bool isPaused = false;
    private List<GameObject> defenses = new List<GameObject>();

    void Start()
    {
        if (pauseText != null)
            pauseText.gameObject.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false); 

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitToMainMenu);

        if (retryButton != null)
            retryButton.onClick.AddListener(QuitToMainMenu);


        // Find all defenses at start
        defenses.AddRange(GameObject.FindGameObjectsWithTag("Defense"));
    }

    void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            if (pauseText != null)
                pauseText.gameObject.SetActive(true);

            if (pausePanel != null)
                pausePanel.SetActive(true);

            DisableUIButtons();
            DisableDefenses(); 
        }
    }

    void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            if (pauseText != null)
                pauseText.gameObject.SetActive(false);

            if (pausePanel != null)
                pausePanel.SetActive(false);

            EnableUIButtons();
            EnableDefenses(); 
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    void DisableUIButtons()
    {
        foreach (Button button in uiButtonsToDisable)
        {
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    void EnableUIButtons()
    {
        foreach (Button button in uiButtonsToDisable)
        {
            if (button != null)
            {
                DeployButton deployButton = button.GetComponent<DeployButton>();

                if (deployButton != null && LevelManager.Instance != null)
                {
                    bool isUnlocked = LevelManager.Instance.unlockedDefenses.Contains(deployButton.defenseData);
                    button.interactable = isUnlocked;
                }
                else
                {
                    button.interactable = true;
                }
            }
        }
    }

    void DisableDefenses()
    {
        defenses.Clear();
        defenses.AddRange(GameObject.FindGameObjectsWithTag("Defense")); // Refresh list

        foreach (GameObject defense in defenses)
        {
            if (defense != null)
            {
                // Disable collider to prevent clicks
                Collider collider = defense.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                // Disable RangeIndicator script
                RangeIndicator rangeIndicator = defense.GetComponent<RangeIndicator>();
                if (rangeIndicator != null)
                {
                    rangeIndicator.enabled = false;
                }
            }
        }
    }

    void EnableDefenses()
    {
        foreach (GameObject defense in defenses)
        {
            if (defense != null)
            {
                // Re-enable collider
                Collider collider = defense.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }

                // Re-enable RangeIndicator script
                RangeIndicator rangeIndicator = defense.GetComponent<RangeIndicator>();
                if (rangeIndicator != null)
                {
                    rangeIndicator.enabled = true;
                }
            }
        }
    }
}
