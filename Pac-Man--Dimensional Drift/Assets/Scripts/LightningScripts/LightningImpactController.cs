using UnityEngine;

public class LightningImpactController : MonoBehaviour
{
    public Transform player;                    // The player character
    public Transform enemyEyes;                 // The origin of the lightning (enemy's eyes)
    public float followRadius = 10f;            // Radius within which the impact point follows the player
    public float damageAmount = 10f;            // Amount of damage dealt by lightning

    private bool isFollowingPlayer = false;     // Determines if the impact point should follow the player

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(enemyEyes.position, player.position);

        // Check if player is within the follow radius
        if (distanceToPlayer <= followRadius)
        {
            isFollowingPlayer = true;
            FollowPlayer();
        }
        else
        {
            isFollowingPlayer = false;
        }
    }

    void FollowPlayer()
    {
        // Set the position of this impact point to match the player position
        if (isFollowingPlayer)
        {
            transform.position = player.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the impact point collides with the player to deal damage
        if (isFollowingPlayer && other.CompareTag("PacMan"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the follow radius for easier debugging
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(enemyEyes.position, followRadius);
    }
}
