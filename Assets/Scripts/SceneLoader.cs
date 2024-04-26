using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Functions to be called when the button is clicked
    public void LoadMainSceneOnClick()
    {
        SceneManager.LoadScene("MansionScene"); 
    }
}
