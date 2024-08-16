using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform player, destination;
    public GameObject Player;
    public Transform cubeToRotate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PacMan"))
        {
            // Deactivate the player
            player.gameObject.SetActive(false);

            cubeToRotate.Rotate(90f, 0f, 0f);

            // Move the player to the destination
            player.position = destination.position;



            // Reactivate the player
            player.gameObject.SetActive(true);
        }
    }
}
