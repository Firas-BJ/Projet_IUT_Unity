using UnityEngine;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private ScoreClient scoreClient;
    private string playerName;
    private System.Random random;

    private void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName");
        scoreClient = FindObjectOfType<ScoreClient>();
        random = new System.Random();
        UpdateScoreText();
    }

    public void EndLevel()
    {
        score = random.Next(1, 51);
        UpdateScoreText();
        scoreClient.AddScore(playerName, score);
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void OnPlayerDeath()
    {
        EndLevel();
    }

    public void OnLevelComplete()
    {
        EndLevel();
    }
}
