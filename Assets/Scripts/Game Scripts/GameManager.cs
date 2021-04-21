using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game over tracks if the game has finished or has not even started
    public static bool gameOver = true;
    // Time survived tracks how long the player survives
    public static float timeSurvived;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;

    private int score;
    private SpawnManager spawnManager;
    private AudioManager audioManager;

    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        // Update time survived as long as the game is playing
        if (!GameManager.gameOver)
        {
            GameManager.timeSurvived += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void IncreaseScore(int scoreValue)
    {
        // Increase score and display new value
        score += scoreValue;
        scoreText.SetText("Score: " + score);
    }

    private void UpdateTimerDisplay()
    {
        // Display time in terms of minutes and seconds
        int minutes = Mathf.FloorToInt(GameManager.timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(GameManager.timeSurvived - minutes * 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.SetText("Time: " + timeString);
    }

    public void StartGame(int difficulty)
    {
        audioManager.Play("GameMusic");
        // Set game over to false (i.e. the game is active)
        GameManager.gameOver = false;
        // Reset the time survived
        GameManager.timeSurvived = 0f;
        // Reset the score
        score = 0;
        // Update score display
        IncreaseScore(0);
        // Display score and timer displays
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        // Start spawning enemies
        spawnManager.StartSpawning(difficulty);
        
    }

    public void GameOver()
    {
        // Set the game to over
        GameManager.gameOver = true;
        // Stop spawning enemies
        spawnManager.StopSpawning();

        // Get the enemy controller scripts
        EnemyController[] enemyControllerScripts = GameObject.FindObjectsOfType<EnemyController>();
        
        // Start the victory animation for each enemy
        for (int i = 0; i < enemyControllerScripts.Length; i++)
        {
            enemyControllerScripts[i].Victory();
        }

        // Display gameover screen
        gameOverUI.SetActive(true);
    }
}
