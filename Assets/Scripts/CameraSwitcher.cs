using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;
    private int currentCameraIndex = 0;
    public GameObject staticCanvas;
    public float staticDuration = 0.5f; // Duration of static effect in seconds

    private void Start()
    {
        // Ensure only the first camera is active at start
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == 0);
        }
    }

    private void Update()
    {
        // Check for number key presses to switch cameras
        for (int i = 0; i < cameras.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // Alpha1 (KeyCode.1) corresponds to index 0, Alpha2 (KeyCode.2) corresponds to index 1, and so on
            {
                StartCoroutine(SwitchCameraWithStatic(i));
                break; // Break out of the loop after finding the corresponding camera 
            }
        }
    }
    private System.Collections.IEnumerator SwitchCameraWithStatic(int newIndex)
    {
        // Show static
        staticCanvas.SetActive(true);

        // Wait for a brief duration
        yield return new WaitForSeconds(staticDuration);

        // Disable static
        staticCanvas.SetActive(false);

        // Switch camera
        SwitchCamera(newIndex);
    }
    private void SwitchCamera(int newIndex)
    {
        if (newIndex == currentCameraIndex)
            return; // If the same camera is selected, do nothing so it doesn't jitter and stuff lol 

        // Disable current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Set new camera index
        currentCameraIndex = newIndex;

        // Enable new camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
