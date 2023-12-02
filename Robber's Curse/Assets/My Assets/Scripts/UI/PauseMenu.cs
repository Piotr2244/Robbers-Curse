using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    public GameObject ControlsPanel;
    public GameObject MainButtons;
    public void Start()
    {
        pauseMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                Pause();
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        ControlsPanel.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void ControlsMenu()
    {
        try
        {
            ControlsPanel.SetActive(true);
            MainButtons.SetActive(false);
        }
        catch
        {
            Debug.Log("Menu buttons not connected");
        }
    }
    public void CloseControlsMenu()
    {
        try
        {
            ControlsPanel.SetActive(false);
            MainButtons.SetActive(true);
        }
        catch
        {
            Debug.Log("Menu buttons not connected");
        }
    }
}
