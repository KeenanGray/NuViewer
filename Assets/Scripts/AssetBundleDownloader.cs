using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEngine.XR;

public class AssetBundleDownloader : MonoBehaviour
{
    public string assetName;
    string bearer = "ei0yF_QKvGAAAAAAAAAAjHNkacTBD9GJtopVch_n_o_5DVoSCvOijYNHE63sPRZx";

    public static void DownloadFileFromDropBox(string filename, bool isSceneAssetBundle = false)
    {
        var path = "{\"path\":\"" + filename + "\"}";
        GameObject.FindObjectOfType<AssetBundleDownloader>().StartCoroutine(
            GameObject.FindObjectOfType<AssetBundleDownloader>().DownloadFile(path, isSceneAssetBundle));
    }

    int FileSize;
    IEnumerator DownloadFile(string path, bool isSceneAssetBundle = false)
    {
        yield return GetFileSize(path);

        var request = new UnityWebRequest("https://content.dropboxapi.com/2/files/download", "POST");

        request.SetRequestHeader("Authorization", "Bearer " + bearer);
        request.SetRequestHeader("Dropbox-API-Arg", path);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(path);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(null);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        string savePath = "";

        // yield return request.SendWebRequest();
        request.SendWebRequest();

        //only show the loading screen if a scene is loading
        //Sometimes we load Vuforia .xml and .dat files - this happens on launch in the background
        if (isSceneAssetBundle)
        {
            //activate the loading canvas
            GameObject.Find("LoadingScreen").GetComponent<Canvas>().enabled = true;
        }

        //while the file downloads
        //update slider with progress
        var slider = GameObject.Find("Slider").GetComponent<Slider>();
        while (request.downloadProgress < 1.0f)
        {
            slider.value = request.downloadProgress;
            yield return null;
        }
        slider.value = 1.0f;

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error " + path);
            Debug.Log(request.error);
        }
        else
        {
            savePath = string.Format("{0}{1}", Application.persistentDataPath, SimpleJSON.JSONNode.Parse(path)["path"].Value);

            //remove '/ios' or '/android' from path
#if  UNITY_EDITOR
            savePath = savePath.Replace("/ios", "");
#elif UNITY_ANDROID
            savePath = savePath.Replace("/android","");
#elif (UNITY_IOS)
            savePath = savePath.Replace("/ios", "");
#endif
            //When download finishes, 

            //save the .dat and .xml files 
            if (!isSceneAssetBundle)
            {
                System.IO.File.WriteAllBytes(savePath, request.downloadHandler.data);
            }
            else //load scene from the webrequest as a datastream (this avoids saving unnecessarily to device)
            {
                var res = request.downloadHandler.data;
                using (var stream = new MemoryStream(res))
                {
                    BundleName = savePath.Split('/')[savePath.Split('/').Length - 1];
                    StartCoroutine(LoadSceneFromAssetBundle(savePath, stream));
                }
            }
        }
    }

    IEnumerator GetFileSize(string path)
    {
        var request = new UnityWebRequest("https://api.dropboxapi.com/2/files/get_metadata", "POST");

        request.SetRequestHeader("Authorization", "Bearer " + bearer);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(path);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        var res = request.downloadHandler.text;
        //parse the json
        var N = SimpleJSON.JSON.Parse(res);
        FileSize = int.Parse(N["size"].Value);
    }

    string BundleName;
    Dictionary<string, AsyncOperation> SceneLoadDict;
    IEnumerator LoadSceneFromAssetBundle(string path, Stream stream)
    {
        SceneLoadDict = new Dictionary<string, AsyncOperation>();
        var ab = AssetBundleManager.loadFromStream(path, stream, 0);

        if (ab != null)
        {
            if (ab.isStreamedSceneAssetBundle)
            {
                //TODO: THIS MAKES THE ASSUMPTION OF 1 Scene per boundle
                string[] scenePaths = ab.GetAllScenePaths();
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);

                var slider = GameObject.Find("Slider").GetComponent<Slider>();

                var async = SceneManager.LoadSceneAsync("XRRig", LoadSceneMode.Additive);

                slider.transform.Find("Fill Area").GetComponentInChildren<UnityEngine.UI.Image>().color = (Color)new Color32(160, 184, 70, 255);
                while (async.progress < 0.9f)
                {
                    slider.value = async.progress;
                    yield return null;
                }
                slider.value = 1.0f;

                async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                slider.transform.Find("Fill Area").GetComponentInChildren<UnityEngine.UI.Image>().color = (Color)new Color32(195, 214, 100, 255);
                while (async.progress < 0.9f)
                {
                    slider.value = async.progress;
                    yield return null;
                }
                slider.value = 1.0f;

                //Load the XR device
                yield return StartCoroutine(LoadDevice());

                SceneManager.UnloadSceneAsync("MenuScene");
            }
            else
            {
                Debug.Log("Could not find or already loaded asset bundle" + path);
            }
        }
        else
        {
            Debug.Log("Could not find asset bundle " + path);
        }
        yield break;
    }
    IEnumerator LoadDevice()
    {
        yield return null;
        GameObject.FindObjectOfType<CanvasManager>().EnableAllXR();
        yield return new WaitForSeconds(1.0f);
        yield break;
    }

}


