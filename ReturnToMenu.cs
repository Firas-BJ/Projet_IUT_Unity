using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu"); // Assurez-vous que "Menu" est le nom exact de votre scène de menu
    }
}
