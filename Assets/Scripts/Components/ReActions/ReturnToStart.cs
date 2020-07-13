using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToStart : MonoBehaviour
{
    Vector3 pos;
    bool hasObjectLoaded = false;

    // Start is called before the first frame update
    public void SetInitialPosition(Transform t)
    {
        pos = t.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < -15.0f)
        {
            transform.localPosition = pos;
        }
    }
    
    void OnEnable()
    {
        if (!hasObjectLoaded)
        {
            hasObjectLoaded = true;
            SetInitialPosition(transform);
        }

    }
}
