using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using TMPro;
using PixelCrushers.DialogueSystem;
using cakeslice;

public class VoiceCommandController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private List<DestinationObject> destinationObjects = new List<DestinationObject>();
    [SerializeField] private float movementThreshold = 0.1f;
    public GameObject player;
    [SerializeField] private TextMeshProUGUI InventoryText;
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
    }

    public void ProcessVoiceCommand(string command)
    {
        
        //DialogueLua.SetVariable("Input", command); // Set variable in Dialogue System
        command = command.ToLower();
        bool commandHandled = false;


        if (command.Contains("look"))
        {
            foreach (var destinationObject in destinationObjects)
            {
                Outline outlineScript = destinationObject.gameObject.GetComponent<Outline>();

                if (outlineScript != null)
                {
                    Debug.Log(destinationObject.gameObject.name + " has the Outline script attached.");
                    // Do something with the object that has the Outline script attached, still figurin' that out loool
                }
                else
                {
                    Debug.Log(destinationObject.gameObject.name + " does not have the Outline script attached.");
                }

            }
        }

        if (command.Contains("examine") || command.Contains("look"))
        {
            foreach (var destinationObject in destinationObjects)
            {
                foreach (var keyword in destinationObject.keywords)
                {
                    if (command.Contains(keyword)) // Check lowercase keyword
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
                                InventoryText.text += item.name;
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

        // Check for move commands
        if (!commandHandled)
        {
            foreach (var destinationObject in destinationObjects)
            {
                foreach (var keyword in destinationObject.keywords)
                {
                    if (command.Contains(keyword.ToLower())) // Check lowercase keyword
                    {
                        // Set destination regardless of interaction
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
    //private void UseItemOnObject(Item item, DestinationObject targetObject)
    //{
    //    // Implement logic for using the item on the object
    //    Debug.Log("Using " + item.name + " on " + targetObject.gameObject.name);

    //    // Remove the item from the inventory
    //    inventory.Remove(item);

    //    // Interact with the target object
    //    targetObject.Interact();
    //}

    private void SetDestination(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
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
            //Debug.Log("Got: " + item);
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
