using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using TMPro;
using System;

public class SaveImageDatabaseToDevice : MonoBehaviour
{
    public string bundleName;
    Button OpenGalleryBtn;
    // Start is called before the first frame update
    void Start()
    {
        bundleName = "";
        GetComponent<Button>().onClick.AddListener(ListVuforiaComponents);
        OpenGalleryBtn = GameObject.Find("OpenGalleryBtn").GetComponent<Button>();
        OpenGalleryBtn.onClick.AddListener(OpenGallery);
    }

    void ListVuforiaComponents()
    {
        StartCoroutine(LoadingPanel());

        if (NativeGallery.CheckPermission() != NativeGallery.Permission.Granted)
        {
            //if we don't have permission we can request again
            NativeGallery.RequestPermission();
        }

        foreach (TurnOffBehaviorCustom x in GameObject.FindObjectsOfType<TurnOffBehaviorCustom>())
        {
            // var asb = AssetBundleManager.getAssetBundle(bundleName, 0);
            Texture tex = null;
            //check if this tracker is used by the scene
            if (x.gameObject.name != "New Game Object" || x.transform.childCount > 0)
            {
                var trackableName = x.GetComponent<ImageTargetBehaviour>().TrackableName;
                Debug.Log("found trackable " + trackableName); //print the name of the trackable

                //get the trackable image
                try
                {
                    tex = x.image;
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                    continue;
                }
                if (tex == null)
                    return;

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
        var datetime = System.DateTime.Now;
        string time = datetime.ToString("dd-mm-yyyy");
        var name = t.name + time + ".png";
        Debug.Log("Saving texure " + name);

        try
        {
            NativeGallery.SaveImageToGallery(t.EncodeToPNG(), "Camera", name, null);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            var rt = RenderTexture.GetTemporary(t.width, t.height, 0);
            Graphics.Blit(t, rt);
            RenderTexture.active = rt;
            Texture2D destination_texture = new Texture2D(t.width, t.height);
            destination_texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            destination_texture.Apply();

            byte[] final;
#if UNITY_ANDROID
            final = destination_texture.EncodeToPNG();
#else
            final = destination_texture.EncodeToPNG();
#endif

            NativeGallery.SaveImageToGallery(final, "Camera", name, null);

        }
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
        OpenGalleryBtn.gameObject.SetActive(false);

        GameObject.Find("LoadingCanvas").GetComponent<Canvas>().enabled = true;

        yield return new WaitForSeconds(1.0f);
        GameObject.Find("SavingText").GetComponent<TextMeshProUGUI>().text = "Saved";

        //set the button visible on IOS 

#if UNITY_IOS
        OpenGalleryBtn.gameObject.SetActive(true);
#endif
        //TODO Add android support to this function

        yield return new WaitForSeconds(3.0f);

        GameObject.Find("LoadingCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("LoadingCanvas").GetComponentInChildren<TextMeshProUGUI>().text = "Saving";
        //OpenGalleryBtn.gameObject.SetActive(false);

        yield break;
    }
}

