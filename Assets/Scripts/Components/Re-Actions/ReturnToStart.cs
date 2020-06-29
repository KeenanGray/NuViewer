using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToStart : MonoBehaviour
{
    Vector3 pos;
    bool hasObjectLoaded = false;

    private void Start()
    {
        SetInitialPosition(transform);

    }
    // Start is called before the first frame update
    public void SetInitialPosition(Transform t)
    {
        pos = t.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y< -5.0f)
        {
            transform.position = pos;
        }
    }
}
