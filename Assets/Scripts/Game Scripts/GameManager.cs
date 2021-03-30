using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO look into usage of gameOver and gameIsActive and refactor.
    public static bool gameOver = false;
    public static bool gameIsActive = false;
    public static float time;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;

    private int score;
    private SpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        GameManager.gameOver = false;
        GameManager.gameIsActive = false;
    }

    private void Update()
    {
        if (!GameManager.gameOver)
        {
            GameManager.time += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void IncreaseScore(int scoreValue)
    {
        score += scoreValue;
        scoreText.SetText("Score: " + score);
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(GameManager.time / 60f);
        int seconds = Mathf.FloorToInt(GameManager.time - minutes * 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.SetText("Time: " + timeString);
    }

    public void StartGame(int difficulty)
    {
        GameManager.gameIsActive = true;
        GameManager.time = 0f;
        score = 0;
        IncreaseScore(0);
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        spawnManager.StartSpawning(difficulty);
    }

    public void GameOver()
    {
        GameManager.gameOver = true;
        GameManager.gameIsActive = false;
        spawnManager.StopSpawning();

        EnemyController[] enemyControllerScripts = GameObject.FindObjectsOfType<EnemyController>();
        
        for (int i = 0; i < enemyControllerScripts.Length; i++)
        {
            enemyControllerScripts[i].Victory();
        }

        // Display gameover screen
        gameOverUI.SetActive(true);
    }
}
