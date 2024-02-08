using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class VoiceCommandController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private List<DestinationObject> destinationObjects = new List<DestinationObject>();
    [SerializeField] private float movementThreshold = 0.1f;

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

    public void ProcessVoiceCommand(string command)
    {
        command = command.ToLower(); // Convert command to lowercase

        bool commandHandled = false;

        foreach (var destinationObject in destinationObjects)
        {
            foreach (var keyword in destinationObject.keywords)
            {
                if (command.Contains(keyword.ToLower())) // Check lowercase keyword
                {
                    if (command.Contains("use"))
                    {
                        // Check if the destination object is interactable
                        if (destinationObject.isInteractable && destinationObject.hasItem)
                        {
                            // Add item to the inventory
                            Item item = destinationObject.GetItem();
                            if (item != null)
                            {
                                inventory.Add(item);
                                Debug.Log("Picked up " + item.name);
                                commandHandled = true;
                            }
                        }
                        else
                        {
                            // Perform other interaction (e.g., open door)
                            destinationObject.Interact();

                            commandHandled = true;
                        }
                    }
                    else
                    {
                        // Set destination regardless of interaction
                        FaceDestination();
                        SetDestination(destinationObject.transform.position);
                        commandHandled = true;
                    }

                    break; // Exit the loop if a matching keyword is found
                }
            }

            if (commandHandled)
                break;
        }

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
}

[System.Serializable]
public class DestinationObject
{
    public Transform transform;
    public List<string> keywords;
    public bool isInteractable = true;
    public bool hasItem = false;
    public Item item; // Reference to the item associated with this object

    public void Interact()
    {
        // Implement interaction behavior here (e.g., open door)
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
