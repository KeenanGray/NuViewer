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
    public static class XRManager
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
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            
#if UNITY_IOS
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
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
#endif

        }
    }
}