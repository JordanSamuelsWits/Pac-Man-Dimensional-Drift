using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FadeTransition : MonoBehaviour
{
    public Image fadeImage; // Reference to the image used for fading
    public TextMeshProUGUI levelText; // Reference to the LevelText
    public float fadeDuration = 1f; // Duration of the fade effect

    private void Start()
    {
        // Set the image and text to be fully transparent at the start
        fadeImage.color = new Color(0, 0, 0, 0);
        levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, 0);
    }

    public IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1); // Ensure fully opaque at the end
        levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, 1); // Ensure text is fully visible
    }

    public IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0); // Ensure fully transparent at the end
        levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, 0); // Hide text
    }
}
