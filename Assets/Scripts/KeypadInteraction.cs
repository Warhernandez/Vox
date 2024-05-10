using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class KeypadInteraction : MonoBehaviour
{
    public GameObject keypadUI;
    public TMP_InputField codeInputField;
    public string correctCode = "1234";
    public Animator doorAnimator; // Reference to the door's Animator component

    public AudioSource keypadClickSound;
    public AudioSource wrongCodeSound;
    public AudioSource rightInputSound;

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

                        // Play the keypad click sound
                        if (keypadClickSound != null)
                            keypadClickSound.Play();
                    }
                }
            }
        }

        // Check for Enter key press to submit code
        if (isKeypadActive && Input.GetKeyDown(KeyCode.Return))
        {
            codeInputField.text = codeInputField.text.ToLower();
            CheckCode(codeInputField.text);
        }
    }

    public void CheckCode(string enteredCode)
    {
        if (enteredCode == correctCode)
        {
            StopAllConversations();
            rightInputSound.Play();
            // Code is correct, play door opening animation
            if (doorAnimator != null)
                doorAnimator.SetTrigger("OpenDoorTrigger");
            DialogueManager.StartConversation("End");
            //FOR DEMO ONLY - SEND PLAYER TO WIN SCENE 
            //SceneManager.LoadScene("ENDOFDEMO");
        }
        else
        {
            // Code is incorrect, play wrong code sound
            if (wrongCodeSound != null)
                wrongCodeSound.Play();

            // Display an error message
            Debug.Log("Incorrect code entered!");
        }

        // Reset the keypad state
        isKeypadActive = false;
        keypadUI.SetActive(false);
        codeInputField.text = ""; // Clear the input field
    }

    public void StopAllConversations()
    {
        for (int i = DialogueManager.instance.activeConversations.Count - 1; i >= 0; i--)
        {
            DialogueManager.instance.activeConversations[i].conversationController.Close();
        }
    }
}
