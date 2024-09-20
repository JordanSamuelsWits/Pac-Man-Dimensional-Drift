using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    public Image fadeImage; // Reference to the image used for fading
    public float fadeDuration = 1f; // Duration of the fade effect

    private void Start()
    {
        // Set the image to be fully transparent at the start
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1); // Ensure fully opaque at the end
    }

    public IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0); // Ensure fully transparent at the end
    }
}
