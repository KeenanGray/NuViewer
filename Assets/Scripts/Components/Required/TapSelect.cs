using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TapSelect : MonoBehaviour
{
    RaycastHit hit;

    bool canTapAgain = true;
    // Update is called once per frame
    void Update()
    {
        var touches = Input.touchCount;
        Touch[] t = Input.touches;

        //#if UnityEditor
        if (Input.GetMouseButton(0))
        {

            touches = 1;
            var touch = new Touch();
            touch.position = Input.mousePosition;
            touch.phase = TouchPhase.Began;

            t = new Touch[] { touch };
        }

        //#endif

        if (touches > 0 && canTapAgain)
        {
            var m_Ray = Camera.main.ScreenPointToRay(t[0].position);

            if (Physics.Raycast(m_Ray.origin, m_Ray.direction, out hit, Mathf.Infinity, -1, QueryTriggerInteraction.Collide))
            {
                try
                {
                    hit.collider.gameObject.SendMessage("Tapped");
                    hit.collider.GetComponentInParent<OnTap>().SendMessage("Tapped");
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            canTapAgain = false;
        }

        if (touches <= 0)
        {
            canTapAgain = true;
        }
    }
}
