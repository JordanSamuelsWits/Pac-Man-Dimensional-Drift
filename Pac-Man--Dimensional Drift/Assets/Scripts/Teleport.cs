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

// Adaptation for our game
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
