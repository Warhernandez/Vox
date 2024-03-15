using UnityEngine;

namespace cakeslice
{
    public class Toggle : MonoBehaviour
    {
        private Outline outline;

        void Start()
        {
            // Get the Outline component attached to this object
            outline = GetComponent<Outline>();
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the mouse ray hits this object
            if (Physics.Raycast(ray, out hit))
            {
                // If the hit object is this object, enable the outline
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Object hit!");
                    outline.enabled = true;
                }
                else
                {
                    // If the mouse is not over this object, disable the outline
                    outline.enabled = false;
                }
            }
            else
            {
                // If the mouse is not pointing at anything, disable the outline!!!!!!
                outline.enabled = false;
            }
        }
    }
}
