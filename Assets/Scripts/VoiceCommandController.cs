using UnityEngine;

public class VoiceCommandController : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject redBlock;
    [SerializeField] private GameObject blueBlock;
    [SerializeField] private GameObject greenBlock;
    [SerializeField] private GameObject yellowBlock;

    private void Start()
    {
        // Initialize the character position
        MoveCharacterToPosition(character.transform.position);
    }

    public void ProcessVoiceCommand(string command)
    {
        command = command.ToLower(); // Convert the command to lowercase for case-insensitive comparison

        if (command.Contains("red"))
        {
            MoveCharacterToPosition(redBlock.transform.position);
        }
        else if (command.Contains("blue"))
        {
            MoveCharacterToPosition(blueBlock.transform.position);
        }
        else if (command.Contains("green"))
        {
            MoveCharacterToPosition(greenBlock.transform.position);
        }
        else if (command.Contains("yellow"))
        {
            MoveCharacterToPosition(yellowBlock.transform.position);
        }
        else
        {
            Debug.Log("Unknown command: " + command);
        }
    }

    private void MoveCharacterToPosition(Vector3 targetPosition)
    {
        // You can use your own logic here to smoothly move the character to the target position
        character.transform.position = targetPosition;
    }
}
