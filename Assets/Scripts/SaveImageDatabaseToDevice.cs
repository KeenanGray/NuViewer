using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using TMPro;

public class SaveImageDatabaseToDevice : MonoBehaviour
{
    public string bundleName;

    // Start is called before the first frame update
    void Start()
    {
        bundleName = "";
        GetComponent<Button>().onClick.AddListener(ListVuforiaComponents);
    }

    void ListVuforiaComponents()
    {
        StartCoroutine(LoadingPanel());

        if (NativeGallery.CheckPermission() != NativeGallery.Permission.Granted)
        {
            //if we don't have permission we can request again
            NativeGallery.RequestPermission();
        }

        if (bundleName == null || bundleName == "")
        {
            Debug.LogError("problem identifying bundle name");
            return;
        }

        foreach (TrackableBehaviour x in Vuforia.TrackerManager.Instance.GetStateManager().GetTrackableBehaviours())
        {
            // var asb = AssetBundleManager.getAssetBundle(bundleName, 0);
            Texture tex = null;
            //check if this tracker is used by the scene
            if (x.gameObject.name != "New Game Object" || x.transform.childCount > 0)
            {
                var trackableName = x.TrackableName;
                Debug.Log("found trackable " + trackableName); //print the name of the trackable

                //get the trackable image
                try
                {
                    tex = x.GetComponent<TurnOffBehaviorCustom>().image;
                }
                catch (System.Exception e)
                {
                    continue;
                }

                var sprite = Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
                Texture2D t = sprite.texture;

                //Instantiate a prefab to display the image target on the screen
                /*
                 var go = Instantiate(Resources.Load("ImageTargetThumb") as GameObject,
                 GameObject.Find("Canvas").transform);

                 go.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height),new Vector2(0,0));

                 */
                StartCoroutine(SaveImages(t));
                //Save each image target to the photo gallery
            }
            //download the trackable image file to the device
        }
    }

    IEnumerator SaveImages(Texture2D t)
    {
        NativeGallery.SaveImageToGallery(t.EncodeToPNG(), "ImageTargets", "Image.png", null);
        yield break;
    }

    void OpenGallery()
    {
#if UNITY_IOS
        Application.OpenURL("photos-redirect://");
#elif UNITY_ANDROID
        //TODO: Figure out how to open file browser on android
#endif
    }

    IEnumerator LoadingPanel()
    {
        var button = GameObject.Find("OpenGalleryBtn").GetComponent<Button>();

        button.onClick.AddListener(OpenGallery);
        button.gameObject.SetActive(false);

        GameObject.Find("Loading").GetComponent<Canvas>().enabled = true;

        yield return new WaitForSeconds(1.0f);
        GameObject.Find("SavingText").GetComponent<TextMeshProUGUI>().text = "Saved";

        //set the button visible on IOS 

#if UNITY_IOS
        button.gameObject.SetActive(true);
#endif
//TODO Add android support to this function

        yield return new WaitForSeconds(3.0f);

        GameObject.Find("Loading").GetComponent<Canvas>().enabled = false;

        GameObject.Find("Loading").GetComponentInChildren<TextMeshProUGUI>().text = "Saving";

        yield break;
    }
}

