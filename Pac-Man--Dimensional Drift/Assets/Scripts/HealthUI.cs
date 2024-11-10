using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthSlider;  // The health slider UI element

    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;   // Set the max value of the health bar
        healthSlider.value = maxHealth;      // Set current value to max health
    }

    public void SetHealth(float currentHealth)
    {
        healthSlider.value = currentHealth; // Update the health slider with the current health
    }
}
