using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Maximum health of the enemy
    private int currentHealth;   // Current health of the enemy

    private void Start()
    {
        currentHealth = maxHealth;  // Initialize current health to max health
    }

    // Method to deal damage to the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Decrease health by damage amount

        if (currentHealth < 0)
        {
            currentHealth = 0;  // Ensure health doesn't go below 0
        }

        // Check if the enemy has died
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method for when the enemy dies
    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);  // Destroy the enemy object
    }
}
