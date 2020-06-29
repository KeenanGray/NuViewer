using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSelect : MonoBehaviour
{
    RaycastHit hit;

    public delegate void GazeChanged(Collider col);
    public static GazeChanged GazeChangedCallback;

    Collider last;

    // Update is called once per frame
    void Update()
    {
        var m_Ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(m_Ray.origin, m_Ray.direction, out hit, Mathf.Infinity, -1, QueryTriggerInteraction.Collide))
        {
            //if we hit a new object, callback to gaze changed
            if (hit.collider != last)
            {
                GazeChangedCallback.Invoke(hit.collider);
            }
            //while we are hitting something, call "Gazed"
            try
            {
                hit.collider.gameObject.SendMessage("Gazed");
            }
            catch
            {

            }
            last = hit.collider;

        }
    }
}
