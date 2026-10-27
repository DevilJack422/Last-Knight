using System;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel, optionPanel, confirmPanel;

    void Start()
    {
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }

    public void ResumeClick()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void LoadClick()
    {
        // TODO: Open Save panel and choose which save slot to load
    }

    public void OptionClick()
    {
        optionPanel.SetActive(true);
    }

    public void MenuClick()
    {
        pausePanel.SetActive(false);
        confirmPanel.SetActive(true);
    }

    public void ConfirmClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void TurnBackClick()
    {
        confirmPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
