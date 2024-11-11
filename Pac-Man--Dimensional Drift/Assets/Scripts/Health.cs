/*using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;    // Max health for the player
    private float currentHealth;      // Current health

    public HealthUI healthUI;         // Reference to the HealthUI script
    public GameOverUI gameOverUI;     // Reference to the GameOverUI script

    void Start()
    {
        currentHealth = maxHealth;   // Initialize health at the start
        healthUI.SetMaxHealth(maxHealth);  // Initialize UI with max health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;  // Subtract damage from current health
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();  // Call the death method when health reaches 0
        }

        healthUI.SetHealth(currentHealth);  // Update the health UI
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Prevent health from going above max
        healthUI.SetHealth(currentHealth);  // Update the health UI
    }

    private void Die()
    {
        // Handle death
        Debug.Log("Player has died.");

        // Trigger the game over UI to show "You Died!" text and buttons
        gameOverUI.ShowGameOverUI();
    }
}
*/

/*
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;    // Max health for the player
    private float currentHealth;      // Current health

    public HealthUI healthUI;         // Reference to the HealthUI script
    public GameOverUI gameOverUI;     // Reference to the GameOverUI script

    void Start()
    {
        currentHealth = maxHealth;   // Initialize health at the start
        healthUI.SetMaxHealth(maxHealth);  // Initialize UI with max health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;  // Subtract damage from current health
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();  // Call the death method when health reaches 0
        }

        healthUI.SetHealth(currentHealth);  // Update the health UI
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Prevent health from going above max
        healthUI.SetHealth(currentHealth);  // Update the health UI
    }

    private void Die()
    {
        // Handle death
        Debug.Log("Player has died.");

        Time.timeScale = 0f;

        // Trigger the game over UI to show "You Died!" text and buttons
        gameOverUI.ShowGameOverUI();
    }
}
*/

using UnityEngine;
using UnityEngine.SceneManagement;  // Import SceneManager

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public HealthUI healthUI;      // Reference to the HealthUI script
    public string gameOverScene;   // Name of the scene to load upon death

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();  // Call the death method when health reaches 0
        }

        healthUI.SetHealth(currentHealth);
    }

    private void Die()
    {
        // Optional: Show game over UI
        Debug.Log("Player has died.");

        // Load the specified game over scene
        SceneManager.LoadScene("GameOverScene");
    }
}
