using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;    // Reference to the "You Died!" text
    public Button restartButton;   // Reference to the Restart Button
    public Button mainMenuButton;  // Reference to the Main Menu Button

    // Method to restart the game (reloads the current scene)
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene
    }

    // Method to load the main menu scene
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Make sure you have a scene named "MainMenu"
    }

    // This method shows the Game Over UI (called when health reaches 0)
    public void ShowGameOverUI()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);
    }
}
