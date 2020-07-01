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
    public StringReference assetName;

    public static void DownloadFileFromDropBox(string filename, bool isSceneAssetBundle = false)
    {
        var path = "{\"path\":\"" + filename + "\"}";
        var abd = GameObject.FindObjectOfType<AssetBundleDownloader>();

        abd.StartCoroutine(
           abd.DownloadFile(path, null, isSceneAssetBundle));
    }

    int FileSize;
    IEnumerator DownloadFile(string path, string bearer = null, bool isSceneAssetBundle = false)
    {
        //yield return GetFileSize(path);

        var request = new UnityWebRequest("https://content.dropboxapi.com/2/files/download", "POST");

        if (bearer == null || bearer == "")
        {
            bearer = getBearerString(this, path, isSceneAssetBundle);
            yield break;
        }

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
            savePath = savePath.Replace("/android", "");
#elif UNITY_ANDROID
            savePath = savePath.Replace("/android","");
#elif UNITY_IOS
            savePath = savePath.Replace("/ios", "");
#endif
            //When download finishes, 

            //save the .dat and .xml files 
            if (!isSceneAssetBundle)
            {
                Debug.Log("saving: " + savePath);
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

    /*
        IEnumerator GetFileSize(string path)
        {
            var request = new UnityWebRequest("https://api.dropboxapi.com/2/files/get_metadata", "POST");

            bearer=getBearerString();
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
    */

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

                var async = SceneManager.LoadSceneAsync("StandardUI", LoadSceneMode.Additive);
                slider.transform.Find("Fill Area").GetComponentInChildren<UnityEngine.UI.Image>().color = (Color)new Color32(160, 184, 70, 255);
                while (async.progress < 0.9f)
                {
                    slider.value = async.progress;
                    yield return null;
                }
                slider.value = 1.0f;

                //can't unload the scene immediately
                //wait for new scene to load
                while (!SceneManager.GetSceneByName("StandardUI").isLoaded)
                {
                    yield return null;
                }
                //set the new active scene so vuforia objects initialize in the right place
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("StandardUI"));

                Vuforia.VuforiaRuntime.Instance.InitVuforia();

                //Set the name of the asset bundle so we can unload it when the scene closes
                assetName.Value = ab.name;

                async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                slider.transform.Find("Fill Area").GetComponentInChildren<UnityEngine.UI.Image>().color = (Color)new Color32(195, 214, 100, 255);
                while (async.progress < 0.9f)
                {
                    slider.value = async.progress;
                    yield return null;
                }
                slider.value = 1.0f;

                async = SceneManager.UnloadSceneAsync("MenuScene");
                while (async.progress < 0.9f)
                {
                    yield return null;
                }
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

    public string getBearerString(object o, string filepath = "", bool m_isSceneAssetBundle = false)
    {
        var val = "";
        var path = "";
#if UNITY_EDITOR
        path = "Assets/configAR.txt";
#else
        path = Application.streamingAssetsPath + "/configAR.txt";
#endif

#if UNITY_IOS || UNITY_EDITOR
        StreamReader reader = new StreamReader(path);
        val = reader.ReadToEnd();
        reader.Close();
#endif

#if UNITY_ANDROID
        val = "";
        path = Application.streamingAssetsPath + "/configAR.txt";

        StartCoroutine(WebRequest(path, returnValue =>
        {
            if (val != null)
            {
                val = returnValue;
                val = val.Split(':')[1];
                val = val.Replace(" ", "");
                val = val.Replace("\n", "");
                if (o.GetType() == typeof(AssetBundleDownloader))
                {
                    var abd = (AssetBundleDownloader)o;
                    abd.StartCoroutine(
                        abd.DownloadFile(filepath, val, m_isSceneAssetBundle));
                }
                else if (o.GetType() == typeof(ReadFilesFromDropBox))
                {
                    var rfdb = (ReadFilesFromDropBox)o;
                    rfdb.ReadFiles(val);
                }
            }
        }));
        return "";
#else
        val = val.Split(':')[1];
        val = val.Replace(" ", "");
        val = val.Replace("\n", "");
        return val;
#endif

    }

    IEnumerator WebRequest(string path, System.Action<string> callback = null)
    {
        var val = "";
        var url = path;
#if UNITY_EDITOR
        url = "file://" + path;//Path.Combine("file://", path);
#endif
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                val = "error " + webRequest.error;
            }
            else
            {
                //Debug.Log(webRequest.downloadHandler);
                val = webRequest.downloadHandler.text;
                callback(val);
            }
        }
    }
}


