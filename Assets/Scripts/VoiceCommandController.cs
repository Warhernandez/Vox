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
    private bool isNearInteractable = false;
    [SerializeField] private Material highlightMaterial; // Material used for highlighting

    private bool isHighlighting = false; // Flag to track highlighting state

    private List<Item> inventory = new List<Item>(); // Inventory list to store collected items

    private void Start()
    {
        SetDestination(navMeshAgent.transform.position);
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
                Debug.Log("trigger enter.");
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
                Debug.Log("trigger exit.");
                break;
            }
        }
    }


    public void ProcessVoiceCommand(string command)
    {
        command = command.ToLower();
        bool commandHandled = false;

        // Split the command into individual words
        string[] words = command.Split(' ');

        // Check for "use" command followed by "key" and "door" (or other objects)
        if (words.Length >= 3 && words[0] == "use")
        {
            string itemName = words[1];
            string targetObjectName = string.Join(" ", words, 2, words.Length - 2);

            // Check if the player has the specified item in their inventory
            Item item = inventory.Find(x => x.name.ToLower() == itemName);
            if (item != null)
            {
                // Check if there is an interactable object with the specified name
                DestinationObject targetObject = destinationObjects.Find(x => x.gameObject.name.ToLower() == targetObjectName);
                if (targetObject != null && targetObject.isInteractable)
                {
                    // Use the item on the target object
                    UseItemOnObject(item, targetObject);
                    return;
                }
                else
                {
                    Debug.Log("Target object not found or is not interactable.");
                }
            }
            else
            {
                Debug.Log("Item not found in inventory.");
            }
        }
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
    private void UseItemOnObject(Item item, DestinationObject targetObject)
    {
        // Implement logic for using the item on the object
        Debug.Log("Using " + item.name + " on " + targetObject.gameObject.name);

        // Remove the item from the inventory
        inventory.Remove(item);

        // Interact with the target object
        targetObject.Interact();
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

    private bool isNearby = false; // Flag to track if the player is nearby

    public bool IsNearby => isNearby;

    public void OnPlayerEnter()
    {
        isNearby = true;
    }

    public void OnPlayerExit()
    {
        isNearby = false;
    }

    public void Interact()
    {
        // Implement interaction behavior here (e.g., open door)
        Debug.Log("Interacting with object: " + gameObject.name);
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
