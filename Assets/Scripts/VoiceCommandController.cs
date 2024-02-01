using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class VoiceCommandController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private List<DestinationObject> destinationObjects = new List<DestinationObject>();
    [SerializeField] private float movementThreshold = 0.1f;

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
        command = command.ToLower();

        foreach (var destinationObject in destinationObjects)
        {
            foreach (var keyword in destinationObject.keywords)
            {
                if (command.Contains(keyword))
                {
                    FaceDestination();
                    SetDestination(destinationObject.transform.position);
                    return; // Exit the loop if a matching keyword is found
                }
            }
        }

        Debug.Log("Unknown command: " + command);
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
}
