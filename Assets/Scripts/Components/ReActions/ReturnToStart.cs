using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToStart : MonoBehaviour
{
    Vector3 startPos;
    Vector3 startRot;
    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.localPosition;
        startRot = transform.eulerAngles;
    }

    public void Activate()
    {
        transform.localPosition=startPos;
        transform.eulerAngles=startRot;
    }
}
