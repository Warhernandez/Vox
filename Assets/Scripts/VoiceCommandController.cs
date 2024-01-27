using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class VoiceCommandController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject redBlock;
    [SerializeField] private GameObject blueBlock;
    [SerializeField] private GameObject greenBlock;
    [SerializeField] private GameObject yellowBlock;

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
        
    }

    public void ProcessVoiceCommand(string command)
    {
        command = command.ToLower();

        if (command.Contains("red"))
        {
            FaceDestination();
            SetDestination(redBlock.transform.position);
        }
        else if (command.Contains("blue"))
        {
            FaceDestination();
            SetDestination(blueBlock.transform.position);
        }
        else if (command.Contains("green"))
        {
            FaceDestination();
            SetDestination(greenBlock.transform.position);
        }
        else if (command.Contains("yellow"))
        {
            FaceDestination();
            SetDestination(yellowBlock.transform.position);
        }
        else
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
