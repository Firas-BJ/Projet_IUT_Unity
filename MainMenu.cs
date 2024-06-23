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
        SceneManager.LoadScene("ScoreDisplay");
    }
    
    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
