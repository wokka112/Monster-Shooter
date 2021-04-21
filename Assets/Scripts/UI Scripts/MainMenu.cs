using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("MainMenuMusic");    
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();

        // Stop the play mode in Unity Editor if playing in that
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
