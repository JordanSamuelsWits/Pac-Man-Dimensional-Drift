using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;

    // Array to hold audio clips for each level
    public AudioClip[] levelAudioClips;

    // Dictionary to map level names to audio clips
    private Dictionary<string, int> levelAudioMapping = new Dictionary<string, int>()
    {
        { "Glitch Pac-Town", 0 },
        { "Forest Maze", 1 },
        { "Desert Ruins", 2 },
        { "Ice Caverns", 3 },
        { "Volcanic Wasteland", 4 },
        { "Techno City", 5 }
    };

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayLevelAudio(GetCurrentLevelName()); // Start playing audio for the current level when the game begins
    }

    // Method to get the current level name
    private string GetCurrentLevelName()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    // Method to play the appropriate audio for a given level
    public void PlayLevelAudio(string levelName)
    {
        if (levelAudioMapping.ContainsKey(levelName))
        {
            int audioIndex = levelAudioMapping[levelName];
            if (audioIndex >= 0 && audioIndex < levelAudioClips.Length)
            {
                audioSource.clip = levelAudioClips[audioIndex];
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for level: " + levelName);
        }
    }

    // Method to stop the current audio
    public void StopCurrentAudio()
    {
        audioSource.Stop();
    }
}
