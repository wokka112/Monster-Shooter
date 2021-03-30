using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButtons : MonoBehaviour
{
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
        difficultyUI.SetActive(false);
        difficultyUIBackground.SetActive(false);
        gameManager.StartGame(difficulty);
    }
}
