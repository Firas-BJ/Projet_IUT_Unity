using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameInputManager : MonoBehaviour
{
    public InputField nameInputField;

    public void StartLevelSelection()
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName); // Sauvegarder le nom du joueur
            SceneManager.LoadScene("LevelSelector"); // Charger la scène de sélection de niveau
        }
    }
}