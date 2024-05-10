using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //public float fadeOutTime = 1.0f;
    //public Animator fadeOutAnimator; // Reference to the fade out animator
    // Functions to be called when the button is clicked
    private bool introclear;
    private bool gameclear;
    private void Update()
    {
        introclear = DialogueLua.GetVariable("IntroQuestClear").AsBool;
        if (introclear)
        {
            SceneManager.LoadScene("MansionScene");
        }

        gameclear = DialogueLua.GetVariable("GameClear").AsBool;
        if (gameclear)
        {
            SceneManager.LoadScene("ENDOFDEMO");
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

    public void LoadTitleSceneOnClick()
    {
        SceneManager.LoadScene("Title");
    }


    // Call this function to change scene after fade out
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}
