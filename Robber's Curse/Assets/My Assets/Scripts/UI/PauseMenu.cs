using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
/* class to pause game and show menu options */
public class PauseMenu : MonoBehaviour
{
    // Variables
    public bool isPaused;
    public bool inControls = false;
    // References
    public GameObject pauseMenu;
    public GameObject ControlsPanel;
    public GameObject MainButtons;
    // Start is called before the first frame update
    public void Start()
    {
        pauseMenu.SetActive(false);
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused && !inControls)
                ResumeGame();
            else if (!isPaused)
                Pause();
        }
    }
    // Pause game and display menu options
    public void Pause()
    {
        pauseMenu.SetActive(true);
        ControlsPanel.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }
    // Return to game and hide menu tab
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        ControlsPanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }
    // Show controls tab
    public void ControlsMenu()
    {
        inControls = true;
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
    // Hide controlls tab
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
        inControls = false;
    }
    // Exit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
