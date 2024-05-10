using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;
    private int currentCameraIndex = 0;
    public GameObject staticCanvas;
    public Image[] miniMaps; // Array of mini-map images
    public float staticDuration = 0.5f; // Duration of static effect in seconds
    public AudioSource staticsound;

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

        //play sound
        staticsound.Play();

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

        //update mini maps
        UpdateMiniMaps(); 
    }

    private void UpdateMiniMaps()
    {
        // Disable all mini-maps
        foreach (var miniMap in miniMaps)
        {
            miniMap.gameObject.SetActive(false);
        }

        // Enable the mini-map corresponding to the current camera
        if (currentCameraIndex >= 0 && currentCameraIndex < miniMaps.Length)
        {
            miniMaps[currentCameraIndex].gameObject.SetActive(true);
        }
    }
}
