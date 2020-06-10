using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class CanvasManager : MonoBehaviour
{
    Transform SelectionScreen;
    Transform LoadingScreen;

    // Start is called before the first frame update
    void Awake()
    {
        KeenanXR.XRManager.DisableAllXR();

        SelectionScreen = transform.GetChild(0);
        LoadingScreen = transform.GetChild(1);

        LoadingScreen.GetComponent<Canvas>().enabled = false;
    }


   
}

