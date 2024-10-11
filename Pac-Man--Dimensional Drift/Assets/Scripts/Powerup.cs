using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float speedBoost = 2.0f; // How much to increase the player's speed
    public float duration = 5.0f;   // How long the power-up lasts

    // When the player collides with the power-up, activate it
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object colliding is the player
        {
            StartCoroutine(PickUp(other));
        }
    }

    IEnumerator PickUp(Collider player)
    {
        // Apply the power-up effect
        FirstPersonControls playerMovement = player.GetComponent<FirstPersonControls>();
        playerMovement.moveSpeed *= speedBoost;

        // Hide the power-up object while it's active
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Wait for the power-up duration
        yield return new WaitForSeconds(duration);

        // Revert the player's speed back to normal
        playerMovement.moveSpeed /= speedBoost;

        // Destroy the power-up object
        Destroy(gameObject);
    }
}
