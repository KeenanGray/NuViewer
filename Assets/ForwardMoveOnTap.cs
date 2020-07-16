using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.XR;

public class ForwardMoveOnTap : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        var touches = Input.touchCount;
#if UNITY_EDITOR
if(Input.GetMouseButton(0))
        touches=1;
#endif
        if (touches > 0)
        {
            Debug.Log("got touch");

            transform.Translate(transform.forward * speed, Space.World);
        }
    }
}
