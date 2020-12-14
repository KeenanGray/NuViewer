using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    GameObject m_ObjectToPlace; 

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();


    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                if(m_RaycastManager.Raycast(touch.position,s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    Instantiate(m_ObjectToPlace, hitPose.position, hitPose.rotation);
                }
            }

        }
    }
}
