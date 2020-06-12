using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    Transform SelectionScreen;
    Transform LoadingScreen;

    // Start is called before the first frame update
    void Awake()
    {
        SelectionScreen = transform.GetChild(0);
        LoadingScreen = transform.GetChild(1);

        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Canvas>().enabled = false;
        }
    }
}

