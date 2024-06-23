using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreDisplay : MonoBehaviour
{
    public List<TextMeshProUGUI> scoreTexts;
    private ScoreClient scoreClient;

    private void Start()
    {
        scoreClient = FindObjectOfType<ScoreClient>();
        if (scoreClient != null)
        {
            List<string> scores = scoreClient.GetScores();
            DisplayScores(scores);
        }
    }

    private void DisplayScores(List<string> scores)
    {
        for (int i = 0; i < scoreTexts.Count && i < scores.Count; i++)
        {
            scoreTexts[i].text = scores[i];
        }

        for (int i = scores.Count; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].text = "";
        }
    }
}
