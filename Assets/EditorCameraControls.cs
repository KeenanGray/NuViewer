using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EditorCameraControls : MonoBehaviour
{
    [Range(.05f, 1)]
    public float speed = 1;
    [Range(1, 20)]
    public float rotSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * (Input.GetAxis("Vertical") * speed), Space.World);
        transform.Rotate(transform.up * (Input.GetAxis("Horizontal") * rotSpeed), Space.World);

        /*
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    var m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(m_Ray.origin, m_Ray.direction, out hit, Mathf.Infinity, -1, QueryTriggerInteraction.Collide))
                    {
                        try
                        {
                            transform.LookAt(hit.point);

                            var angle = 0;
                            if (transform.position.y < hit.point.y)
                                angle = 10;
                            else
                                angle = -10;

                            transform.Rotate(transform.right * angle, Space.World);
                        }
                        catch (Exception e)
                        {

                        }                    
                    */
    }
}

