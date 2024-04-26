using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public float fadeInTime = 1.0f;
    public float displayTime = 2.0f;
    public float fadeOutTime = 1.0f;

    private Image splashImage;

    void Start()
    {
        splashImage = GetComponent<Image>();
        splashImage.canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(ShowSplashScreen());
    }

    IEnumerator ShowSplashScreen()
    {
        splashImage.CrossFadeAlpha(1.0f, fadeInTime, false);
        yield return new WaitForSeconds(fadeInTime + displayTime);
        splashImage.CrossFadeAlpha(0.0f, fadeOutTime, false);
        yield return new WaitForSeconds(fadeOutTime);
        // Load your main menu scene or whatever scene you want to load next
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
