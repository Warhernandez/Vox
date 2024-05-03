using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //public float fadeOutTime = 1.0f;
    //public Animator fadeOutAnimator; // Reference to the fade out animator
    // Functions to be called when the button is clicked
    private bool clear;
    private void Update()
    {
        clear = DialogueLua.GetVariable("IntroQuestClear").AsBool;
        if (clear)
        {
            SceneManager.LoadScene("MansionScene");
        }
    }
    public void LoadMainSceneOnClick()
    {
        SceneManager.LoadScene("MansionScene"); 
    }

    public void LoadIntroSceneOnClick()
    {
        SceneManager.LoadScene("IntroScene");
    }
    

    // Call this function to change scene after fade out
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}
