using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance;

    private void Awake()
    {
        // Ensure there's only one GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make sure this persists across scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to start the countdown in the GameManager
    public void StartPortalCountdown(int level, float delay, Teleport teleportScript)
    {
        StartCoroutine(PortalCountdown(level, delay, teleportScript));
    }

    private IEnumerator PortalCountdown(int level, float delay, Teleport teleportScript)
    {
        Debug.Log($"Starting countdown for EntryPortalL{level}: {delay} seconds.");
        yield return new WaitForSeconds(delay);

        // Activate the portal in the associated Teleport script
        Debug.Log($"Activating EntryPortalL{level}.");
        teleportScript.ActivateEntryPortal();
    }

    // Method to rotate the cube
    /*
    public void RotateCube(Transform cubeToRotate, float duration)
    {
        StartCoroutine(RotateCubeCoroutine(cubeToRotate, duration));
    }
    */
    /*
    private IEnumerator RotateCubeCoroutine(Transform cubeToRotate, float duration)
    {
        Quaternion initialRotation = cubeToRotate.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 90f, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cubeToRotate.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cubeToRotate.rotation = targetRotation;
    }*/
}
