using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;

    public GameObject pauseMenuBackground;
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        // Hide the pause menu
        pauseMenuBackground.SetActive(false);

        // Unpause the game
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        // Display the pause menu
        pauseMenuBackground.SetActive(true);

        // Activate the pause menu's UI as well just incase it is deactivated
        pauseMenuUI.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        // Unpause the game before returning to the main menu just to be safe
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();

        // Turns off play mode if playing in the Unity editor.
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
