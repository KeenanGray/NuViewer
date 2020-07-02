using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Google.XR;
using Google.XR.Cardboard;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace KeenanXR
{
    public class XRManager : MonoBehaviour
    {
        static List<XRDisplaySubsystemDescriptor> displaysDescs = new List<XRDisplaySubsystemDescriptor>();
        static List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();

        static bool IsActive()
        {
            displaysDescs.Clear();
            SubsystemManager.GetSubsystemDescriptors(displaysDescs);

            // If there are registered display descriptors that is a good indication that VR is most likely "enabled"
            return displaysDescs.Count > 0;
        }

        public static bool IsVrRunning()
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

        public static void DisableAllXR()
        {

#if UNITY_IOS
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
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {

                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
                XRGeneralSettings.Instance.Manager.StopSubsystems();
            }
#endif
        }

        public static void EnableAllXR()
        {
            var go = GameObject.FindObjectOfType<XRManager>();
            go.StartCoroutine(go.EnableXR());
        }

        public IEnumerator EnableXR()
        {
//            XRGeneralSettings.Instance.Manager.InitializeLoader();
            while (XRGeneralSettings.Instance == null)
            {
                Debug.Log("no xr settings");
                yield return null;
            }
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
            //   XRGeneralSettings.Instance.Manager.StartSubsystems();

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
            while (!XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                Debug.Log("initing system");
                yield return null;
            }
            Debug.Log("Starting sytem");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            yield break;

        }

    }
}
