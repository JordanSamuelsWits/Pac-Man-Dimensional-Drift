/*using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("MainGame"); // Replace with your main game scene's name
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // This will quit the game if you're running a build
    }
}
*/

/*using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public AudioClip gameOverAudio; // Audio clip to play on game over
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the AudioSource
        audioSource.clip = gameOverAudio;
        audioSource.playOnAwake = false; // Prevent auto-playing if unnecessary
        audioSource.loop = false; // Play only once

        // Play the game over audio
        if (gameOverAudio != null)
        {
            audioSource.Play();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainGame"); // Replace with your main game scene's name
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene's name
    }
}
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public AudioClip firstAudio;  // First audio clip to play
    public AudioClip secondAudio; // Second audio clip to play
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Play the first audio
        if (firstAudio != null)
        {
            audioSource.clip = firstAudio;
            audioSource.Play();
        }

        // Schedule the second audio to play after 2 seconds
        if (secondAudio != null)
        {
            Invoke(nameof(PlaySecondAudio), 2f);
        }
    }

    private void PlaySecondAudio()
    {
        audioSource.clip = secondAudio;
        audioSource.Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainGame"); // Replace with your main game scene's name
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene's name
    }
}
