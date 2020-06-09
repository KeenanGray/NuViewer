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
        DisableAllXR();

        SelectionScreen = transform.GetChild(0);
        LoadingScreen = transform.GetChild(1);

        LoadingScreen.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {

    }


    List<XRDisplaySubsystemDescriptor> displaysDescs = new List<XRDisplaySubsystemDescriptor>();
    List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();

    bool IsActive()
    {
        displaysDescs.Clear();
        SubsystemManager.GetSubsystemDescriptors(displaysDescs);

        // If there are registered display descriptors that is a good indication that VR is most likely "enabled"
        return displaysDescs.Count > 0;
    }

    public bool IsVrRunning()
    {
        bool vrIsRunning = false;
        displays.Clear();
        SubsystemManager.GetInstances(displays);
        foreach (var displaySubsystem in displays)
        {
            if (displaySubsystem.running)
            {
                vrIsRunning = true;
                break;
            }
        }

        return vrIsRunning;
    }

    public void DisableAllXR()
    {
        displays.Clear();
        SubsystemManager.GetInstances(displays);
        foreach (var displaySubsystem in displays)
        {
            if (displaySubsystem.running)
            {
                displaySubsystem.Stop();
                break;
            }
        }

    }

    public void EnableAllXR()
    {
        displays.Clear();
        SubsystemManager.GetInstances(displays);
        foreach (var displaySubsystem in displays)
        {
            if (!displaySubsystem.running)
            {
                displaySubsystem.Start();
                break;
            }
        }

    }
}

