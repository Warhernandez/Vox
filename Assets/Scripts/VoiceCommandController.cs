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

    private void Start()
    {
        SetDestination(navMeshAgent.transform.position);
    }

    public void ProcessVoiceCommand(string command)
    {
        command = command.ToLower();

        if (command.Contains("red"))
        {
            SetDestination(redBlock.transform.position);
        }
        else if (command.Contains("blue"))
        {
            SetDestination(blueBlock.transform.position);
        }
        else if (command.Contains("green"))
        {
            SetDestination(greenBlock.transform.position);
        }
        else if (command.Contains("yellow"))
        {
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

        // Set IsMoving parameter based on whether the agent is moving
        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        // Set the speed parameter for better control over the blend tree
        animator.SetFloat("Speed", isMoving ? 1f : 0f);
    }
}
