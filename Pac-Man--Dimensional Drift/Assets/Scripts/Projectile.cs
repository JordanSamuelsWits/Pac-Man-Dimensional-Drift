using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 20;         // Damage dealt to the player
    public float lifetime = 5f;     // How long the projectile lasts before being destroyed

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the projectile after a certain time
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PacMan"))
        {
            // If it hits the player, apply damage
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the projectile on impact
        }
    }
}
