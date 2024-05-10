using PixelCrushers;
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
    public SceneFader sceneFader;

    public void Start()
    {
        introclear = false;
        gameclear = false; 
    }
    private void Update()
    {
        introclear = DialogueLua.GetVariable("IntroQuestClear").AsBool;
        if (introclear && SceneManager.GetActiveScene().name != "mansionscene")
        {

            sceneFader.FadeToScene("mansionscene");
            
        }

        gameclear = DialogueLua.GetVariable("GameClear").AsBool;
        if (gameclear && SceneManager.GetActiveScene().name != "ENDOFDEMO")
        {
            sceneFader.FadeToScene("ENDOFDEMO");
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
        //StopAllConversations();
        SceneManager.LoadScene("Title");
    }


    // Call this function to change scene after fade out
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StopAllConversations()
    {
        for (int i = DialogueManager.instance.activeConversations.Count - 1; i >= 0; i--)
        {
            DialogueManager.instance.activeConversations[i].conversationController.Close();
        }
    }

}
