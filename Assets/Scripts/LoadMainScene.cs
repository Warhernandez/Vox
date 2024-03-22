using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    // Function to be called when the button is clicked
    public void LoadMainSceneOnClick()
    {
        SceneManager.LoadScene("MansionScene"); // Replace "MainScene" with the name of your main scene
    }
}
