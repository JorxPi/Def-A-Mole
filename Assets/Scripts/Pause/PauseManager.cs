using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public Button pauseButton;   
    public GameObject pauseText;
    public List<Button> uiButtonsToDisable;

    private bool isPaused = false;

    void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame); 
        }

        if (pauseText != null)
        {
            pauseText.SetActive(false);
        }
    }

    void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseText.SetActive(true);
            DisableUIButtons();
        }
    }

    void UnpauseGame()
    {
        if (isPaused) 
        {
            isPaused = false;
            Time.timeScale = 1f;
            pauseText.SetActive(false);
            EnableUIButtons();
        }
    }

    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            UnpauseGame();
        }
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
                button.interactable = true;
            }
        }
    }
}

