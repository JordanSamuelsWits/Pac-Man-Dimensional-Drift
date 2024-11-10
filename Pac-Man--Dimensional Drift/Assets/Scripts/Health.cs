using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Death UI Elements")]
    public GameObject deathMessage;
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        currentHealth = maxHealth;

        // Hide death UI at the start
        if (deathMessage != null)
        {
            deathMessage.SetActive(false);
        }
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.gameObject.SetActive(false);
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Die()
    {
        // Show death message and buttons
        if (deathMessage != null) deathMessage.SetActive(true);
        if (restartButton != null) restartButton.gameObject.SetActive(true);
        if (mainMenuButton != null) mainMenuButton.gameObject.SetActive(true);

        Debug.Log($"{gameObject.name} has died.");

        // Disable player controls or other gameplay elements here if needed
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // Method to restart the game
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    // Method to return to main menu
    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with the main menu scene name
    }
}