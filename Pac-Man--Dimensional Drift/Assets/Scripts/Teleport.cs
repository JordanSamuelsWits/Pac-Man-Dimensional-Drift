// Reference: Omogonix.(Jan 27, 2022) How to Teleport in Unity | Unity Tutorial - https://youtu.be/2IDrPmGf7Mg?si=uQWJrQRXg8kljj06
// Reference: https://docs.unity3d.com/ScriptReference/Quaternion-ctor.html

/* Source Code:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform player, destination;
 public GameObject playerg;
 
 void OnTriggerEnter(Collider other){
  if(other.CompareTag("Player")){
   playerg.SetActive(false);
   player.position = destination.position;
   playerg.SetActive(true);
  }
 }
}
*/

//---------------------------BackUp Code----------------------------------------------------------------------------------------------------------
// Adaptation for our game
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform player, destination;
    public GameObject Player;
    public Transform cubeToRotate; // 3D MAZE CUBE TO ROTATE
    public float rotationDuration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PacMan"))
        {
            // Deactivate the player
            player.gameObject.SetActive(false);

            // Since we are timing this event we need a coroutine
            StartCoroutine(RotateCube());

            // Then we teleport player to the destination
            StartCoroutine(TeleportPlayerAfterRotation());

            //cubeToRotate.Rotate(90f, 0f, 0f);  Rotation about the x-axis

            // Move the player to the destination
            //player.position = destination.position;

            // Reactivate the player
            // player.gameObject.SetActive(true);
        }
    }

    private IEnumerator RotateCube()
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = cubeToRotate.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(90f, 0, 0);

        while (elapsedTime < rotationDuration)
        {
            cubeToRotate.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cubeToRotate.rotation = targetRotation; // Making sure that the final rotation is applied
    }

    private IEnumerator TeleportPlayerAfterRotation()
    {
        // We gotta wait until the rotation is complete
        yield return new WaitForSeconds(rotationDuration);

        // Move the player to the destination
        player.position = destination.position;

        // Reactivate the player :)
        player.gameObject.SetActive(true);
    }
}
*/
//-------------------------------------------------------------------------------------------------------------------------------------

/*
So the goal is to be able to see the cube rotating when we exit the portal and falling onto the 3DCUBEMAZE
But we've disabled the gameObject which then also means we've disabled the camera and the movement of PacMan,
so lets add a few more methods to try and fix this
 */

// We've now added functionality of Portals to ALL levels, with a timing sequence for each level's entryportal

using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform player;
    public Transform destination; // Destination portal location
    public GameObject Player;
    public Transform cubeToRotate;
    public float rotationDuration = 2f; // Duration for the cube rotation
    public float fallDuration = 0.5f; // Time it takes for the player to fall onto the cube
    public FadeTransition fadeTransition;

    private FirstPersonControls playerControls; // Reference to the player's control script
    private bool isPortalActive = false;

    private void Start()
    {
        playerControls = Player.GetComponent<FirstPersonControls>(); // Initialize the player controls reference

        // Start the countdown from the GameManager
        int level = GetLevelNumber(); // Implement this method to get the current level number (1, 2, 3, etc.)
        GameManager.Instance.StartPortalCountdown(level, 60f, this);  // Start countdown for the current portal
    }

    private int GetLevelNumber()
    {
        // Assuming your EntryPortal names follow a pattern like EntryPortalL1, EntryPortalL2, etc.
        string portalName = gameObject.name;
        string levelString = portalName.Replace("EntryPortalL", ""); // Get the level number from the name
        return int.Parse(levelString); // Convert it to an integer
    }

    public void ActivateEntryPortal()
    {
        isPortalActive = true;  // Mark the portal as active
        Debug.Log($"{gameObject.name} activated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPortalActive && other.transform == player)
        {
            // Disable player controls briefly to prevent any unintended actions during teleportation
            playerControls.enabled = false;

            // Teleport the player to the destination and start the cube rotation
            StartCoroutine(TeleportAndRotate());
        }
    }

    private IEnumerator TeleportAndRotate()
    {
        yield return fadeTransition.FadeOut(); // Start fading out

        // Teleport the player to the destination portal's position
        player.position = destination.position;

        // Allow the player to fall onto the cube naturally
        yield return new WaitForSeconds(fallDuration);

        // Re-enable player controls so the player can look around while the cube rotates
        playerControls.enabled = true;

        // Start rotating the cube via GameManager
        GameManager.Instance.RotateCube(cubeToRotate, rotationDuration);

        yield return fadeTransition.FadeIn(); // Start fading in
    }
}






