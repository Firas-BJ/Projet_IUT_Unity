using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("NameInput");
    }

    public void ViewScores()
    {
        SceneManager.LoadScene("ScoreScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
