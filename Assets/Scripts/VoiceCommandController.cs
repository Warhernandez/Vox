using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using TMPro;

public class VoiceCommandController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private List<DestinationObject> destinationObjects = new List<DestinationObject>();
    [SerializeField] private float movementThreshold = 0.1f;

    public GameObject player;
    public GameObject noteImage;
    private bool isNoteEnabled = false;
    private bool isNearInteractable = false;
    private List<Item> inventory = new List<Item>();

    private void Start()
    {
        SetDestination(navMeshAgent.transform.position);
        // Disable the note image initially
        noteImage.SetActive(false);
    }

    private void Update()
    {
        // Check if the character is moving based on a threshold
        bool isMoving = navMeshAgent.velocity.magnitude > movementThreshold;
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("Speed", Mathf.Clamp(navMeshAgent.velocity.magnitude / movementThreshold, 0f, 1f));

        // Face the destination if moving
        if (isMoving)
        {
            FaceDestination();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var destinationObject in destinationObjects)
        {
            if (other.gameObject == destinationObject.gameObject)
            {
                isNearInteractable = true;
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var destinationObject in destinationObjects)
        {
            if (other.gameObject == destinationObject.gameObject)
            {
                isNearInteractable = false;
                break;
            }
        }
    }

    public void ProcessVoiceCommand(string command)
    {
        command = command.Trim().ToLower(); // Convert command to lowercase and trim whitespace

        bool commandHandled = false;

        // Check for "use" command separately
        if (command.StartsWith("use"))
        {
            foreach (var destinationObject in destinationObjects)
            {
                foreach (var keyword in destinationObject.keywords)
                {
                    if (command.Contains(keyword.ToLower())) // Check lowercase keyword
                    {
                        // Check if the player is near an interactable object and if it has an item
                        if (destinationObject.isInteractable && destinationObject.hasItem)
                        {
                            // Add item to the inventory
                            Item item = destinationObject.GetItem();
                            if (item != null)
                            {
                                inventory.Add(item);
                                Debug.Log("Picked up " + item.name);
                                commandHandled = true;
                                break;
                            }
                        }
                        else
                        {
                            Debug.Log("No interactable object nearby or object has no item.");
                            commandHandled = true;
                            break;
                        }
                    }
                }
                if (commandHandled)
                    break;
            }
        }

        // Check for other commands
        if (!commandHandled)
        {
            foreach (var destinationObject in destinationObjects)
            {
                foreach (var keyword in destinationObject.keywords)
                {
                    if (command.Contains(keyword.ToLower())) // Check lowercase keyword
                    {
                        // Set destination regardless of interaction
                        FaceDestination();
                        SetDestination(destinationObject.transform.position);
                        commandHandled = true;
                        break;
                    }
                }
                if (commandHandled)
                    break;
            }
        }

        // Log unknown command if not handled
        if (!commandHandled)
        {
            Debug.Log("Unknown command: " + command);
        }
    }

    private void SetDestination(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    private void FaceDestination()
    {
        Vector3 direction = (navMeshAgent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    //private void InteractWithNote()
    //{
    //    isNoteEnabled = !isNoteEnabled; // Toggle note state

    //    // Enable/disable note image and button
    //    noteImage.sprite = isNoteEnabled ? noteSprite : null;
    //    noteButton.interactable = isNoteEnabled;
    //}
}

[System.Serializable]
public class DestinationObject
{
    public GameObject gameObject; // Reference to the game object
    public Transform transform;
    public List<string> keywords;
    public bool isInteractable = true;
    public bool hasItem = false;
    public Item item; // Reference to the item associated with this object

    public void Interact()
    {
        // Implement interaction behavior here (I.E. open door)
        Debug.Log("Interacting with object");
    }

    public Item GetItem()
    {
        if (hasItem)
        {
            hasItem = false; // Remove the item from the object
            return item;
        }
        return null;
    }
}

[System.Serializable]
public class Item
{
    public string name;
    // Add any other properties relevant to the item
}
