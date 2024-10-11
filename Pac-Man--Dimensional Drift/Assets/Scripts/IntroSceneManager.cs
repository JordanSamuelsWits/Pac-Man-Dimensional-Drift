using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
//using TMPro;
using UnityEngine.UI;
using System.Collections;

public class IntroSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer component
    public Button startButton;       // Reference to the Start Button  
    public float buttonDelay = 80f;  // Delay in seconds before button appears

    void Start()
    {
        // Start playing the video when the scene starts
        videoPlayer.Play();

        // Disable the button initially
        startButton.gameObject.SetActive(false);

        // Start the coroutine to enable the button after delay
        StartCoroutine(EnableButtonAfterDelay(buttonDelay));


    }

    IEnumerator EnableButtonAfterDelay(float delay)
    {
        // Wait for the specified delay (80 seconds)
        yield return new WaitForSeconds(delay);

        // Enable the button
        startButton.gameObject.SetActive(true);

    }

    public void OnStartButtonPressed()
    {
        // Load the main game scene when button is pressed
        SceneManager.LoadScene("MainGame");
    }
}

/*
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class IntroSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer component
    public Button startButton;       // Reference to the Start Button  
    public float buttonDelay = 80f;  // Delay in seconds before button appears
    private Audio audioManager;      // Reference to the Audio script

    void Start()
    {
        // Start playing the video when the scene starts
        videoPlayer.Play();

        // Disable the button initially
        startButton.gameObject.SetActive(false);

        // Start the coroutine to enable the button after delay
        StartCoroutine(EnableButtonAfterDelay(buttonDelay));

        // Find the audio manager in the scene
        audioManager = FindObjectOfType<Audio>();
    }

    IEnumerator EnableButtonAfterDelay(float delay)
    {
        // Wait for the specified delay (80 seconds)
        yield return new WaitForSeconds(delay);

        // Enable the button
        startButton.gameObject.SetActive(true);
    }

    public void OnStartButtonPressed()
    {
        // Ensure AudioL1 for Glitch Pac-Town starts playing when loading the MainGame scene
        if (audioManager != null)
        {
            audioManager.PlayLevelAudio("Glitch Pac-Town");
        }

        // Load the main game scene when button is pressed
        SceneManager.LoadScene("MainGame");
    }
}
*/