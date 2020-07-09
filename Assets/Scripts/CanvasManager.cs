using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class CanvasManager : MonoBehaviour
{
    Transform SelectionScreen;
    Transform LoadingScreen;

    // Start is called before the first frame update
    void Awake()
    {
        SelectionScreen = transform.GetChild(0);
        LoadingScreen = transform.GetChild(1);

        LoadingScreen.GetComponent<Canvas>().enabled = false;

    }
    void Start()
    {
        StartCoroutine("disableXR");
    }

    IEnumerator disableXR()
    {
        while (true)
        {
            if (XRGeneralSettings.Instance != null)
            {
                KeenanXR.XRManager.DisableAllXR();
                break;
            }
            else
                yield return null;
        }
    }
}

