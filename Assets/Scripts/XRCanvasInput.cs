using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.XR.Cardboard;

public class XRCanvasInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Configures the app to not shut down the screen and sets the brightness to maximum.
        // Brightness control is expected to work only in iOS, see:
        // https://docs.unity3d.com/ScriptReference/Screen-brightness.html.
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;

        // Checks if the device parameters are stored and scans them if not.
        if (!Api.HasDeviceParams())
        {
            Api.ScanDeviceParams();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Api.IsGearButtonPressed)
        {
            Debug.Log("Gear Button" + Api.IsGearButtonPressed);
            Api.ScanDeviceParams();
        }

        if (Api.IsCloseButtonPressed)
        {
            Debug.Log("Closed Button " + Api.IsCloseButtonPressed);
            Application.Quit();
        }

        if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }
    }
}
