using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.XR.Cardboard;

namespace KeenanXR
{
    public class CardboardCanvasControls : MonoBehaviour
    {
        public StringReference abName;
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

            ///Since api calls don't seem to be working for these button presses
            //instead check position of touch is aligned to rect
#if UNITY_EDITOR
            if (Input.GetKey("x"))
            {
                CloseButtonPressed();
            }
#endif
            if (Input.touchCount > 0)
            {
                var pos = Input.touches[0].position;

                if ((pos.x > Widget.CloseButtonRect.x &&
                pos.x < Widget.CloseButtonRect.x + Widget.CloseButtonRect.width &&
                pos.y > Widget.CloseButtonRect.y &&
                pos.y < Widget.CloseButtonRect.y + Widget.CloseButtonRect.height)
                || Api.IsCloseButtonPressed)
                {
                    CloseButtonPressed();
                }

                if ((pos.x > Widget.GearButtonRect.x &&
                pos.x < Widget.GearButtonRect.x + Widget.GearButtonRect.width &&
                pos.y > Widget.GearButtonRect.y &&
                pos.y < Widget.GearButtonRect.y + Widget.GearButtonRect.height)
                || Api.IsGearButtonPressed)
                {
                    GearButtonPressed();
                }
            }

            if (Api.HasNewDeviceParams())
            {
                Api.ReloadDeviceParams();
            }
        }

        void CloseButtonPressed()
        {
            //Disable XR
            KeenanXR.XRManager.DisableAllXR();
            //Load the Menu Scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            //Unload the asset bundle
            AssetBundleManager.Unload(abName.Value, 0, true);
        }

        void GearButtonPressed()
        {
            Api.ScanDeviceParams();
        }
    }
}