using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLocationUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI LocationText;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger");

        if (other.CompareTag("Room"))
        {
            string roomName = other.gameObject.name;

            LocationText.text = "Current Location: " + roomName;

            Debug.Log("Updated location text: " + LocationText.text);
        }
    }
}
