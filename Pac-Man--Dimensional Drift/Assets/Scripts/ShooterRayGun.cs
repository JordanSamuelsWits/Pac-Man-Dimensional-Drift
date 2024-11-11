using UnityEngine;

public class ShooterRayGun: MonoBehaviour
{
    public GameObject projectilePrefab;    // The projectile to shoot
    public Transform firePoint;            // The point from where the projectile will be shot
    public float shootInterval = 2f;       // Time interval between each shot
    public float projectileSpeed = 20f;    // Speed of the projectile

    private Transform player;              // Reference to the player

    private void Start()
    {
        // Find the player by tag (assuming the player is tagged as "Player")
        player = GameObject.FindGameObjectWithTag("PacMan").transform;

        // Start repeatedly calling the Shoot function every shootInterval seconds
        InvokeRepeating(nameof(Shoot), shootInterval, shootInterval);
    }

    private void Shoot()
    {
        if (player == null) return;

        // Calculate direction to the player
        Vector3 direction = (player.position - firePoint.position).normalized;

        // Instantiate the projectile and set its position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));

        // Set the projectile's velocity to move towards the player
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }
}
