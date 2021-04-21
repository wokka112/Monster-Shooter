using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButtons : MonoBehaviour
{
    // Set by the difficulty button the script is attached to and determines game difficulty when button is clicked
    public int difficulty;
    public GameObject difficultyUI;
    public GameObject difficultyUIBackground;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SelectDifficulty()
    {
        // Hide the difficuly UI
        difficultyUI.SetActive(false);
        difficultyUIBackground.SetActive(false);
        // Start the game with the difficulty selected via the difficulty UI
        gameManager.StartGame(difficulty);
    }
}
