using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class LookAtNearby : MonoBehaviour
{
    public Transform headTransform;
    public Transform aimtargetTransform;
    public PointOfInterest pointOfInterest;

    public Vector3 origin;
    public float visionRadius;
    public float lerpSpeed; //Lerp is a funny ass word lmao

    // Start is called before the first frame update
    void Start()
    {
        origin = aimtargetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(headTransform.position + transform.forward, visionRadius);

        pointOfInterest = null;

        foreach (Collider col in cols)
        {
            if (col.GetComponent<PointOfInterest>())
            {
                pointOfInterest = col.GetComponent<PointOfInterest>();
                break;
            }
        }


        Vector3 targetPosition;
        if(pointOfInterest != null)
        {
            targetPosition = pointOfInterest.GetLookTarget().position;
        }

        else
        {
            targetPosition = origin;    
        }

        float speed = lerpSpeed / 10;
        aimtargetTransform.position = Vector3.Lerp(aimtargetTransform.position, targetPosition, Time.deltaTime * speed);


    }

    private void OnDragGizmoos()
    {
        Gizmos.color = Color.green * 0.5f;
        Gizmos.DrawWireSphere(headTransform.position + transform.forward, visionRadius);
    }
}
