using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage; // Reference to the UI image used for fading
    public float fadeSpeed = 1.0f; // Speed of the fade

    private bool isFading = false; // Flag to check if fade is in progress

    // Call this function to start the fade effect
    public void FadeToScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            Debug.LogWarning("Scene " + sceneName + " is already loaded.");
            return;
        }

        StartCoroutine(FadeOutRoutine(sceneName));
    }

    IEnumerator FadeOutRoutine(string sceneName)
    {
        // Set flag to indicate fade is in progress
        isFading = true;

        // Fade out
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Load new scene
        SceneManager.LoadScene(sceneName);

        // Fade in
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Reset flag after fade is complete
        isFading = false;
    }

    void Start()
    {
        // Optionally, you can start with a fade-in effect
        fadeImage.color = new Color(0, 0, 0, 1); // Set initial color to fully opaque
        FadeIn();
    }

    // Call this function to trigger a fade-in effect
    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
