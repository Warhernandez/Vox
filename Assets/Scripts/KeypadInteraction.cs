using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class KeypadInteraction : MonoBehaviour
{
    public GameObject keypadUI;
    public TMP_InputField codeInputField;
    public string correctCode = "1234";
    public GameObject door;


    private bool isKeypadActive = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // If the player clicks the keypad, activate the input field
                    if (!isKeypadActive)
                    {
                        isKeypadActive = true;
                        keypadUI.SetActive(true);
                        codeInputField.Select(); // Focus on the input field
                    }
                }
            }
        }

        // Check for Enter key press to submit code
        if (isKeypadActive && Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode(codeInputField.text);
        }
    }

    public void CheckCode(string enteredCode)
    {
        if (enteredCode == correctCode)
        {
            // Code is correct, unlock the door
            //door.SetActive(false);

            //FOR DEMO ONLY - SEND PLAYER TO WIN SCENE 
            SceneManager.LoadScene("ENDOFDEMO"); 
        }
        else
        {
            // Code is incorrect, do something (e.g., display an error message)
            Debug.Log("Incorrect code entered!");
        }

        // Reset the keypad state
        isKeypadActive = false;
        keypadUI.SetActive(false);
        codeInputField.text = ""; // Clear the input field
    }
}
