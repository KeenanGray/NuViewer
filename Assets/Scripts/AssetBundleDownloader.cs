using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.IO;

public class AssetBundleDownloader : MonoBehaviour
{
    public string assetName;
    // Start is called before the first frame update
    void Start()
    {

    }

    public static void DownloadFileFromDropBox(string filename, bool isSceneAssetBundle = false)
    {
        var path = "{\"path\":\"" + filename + "\"}";
        GameObject.FindObjectOfType<AssetBundleDownloader>().StartCoroutine(
            GameObject.FindObjectOfType<AssetBundleDownloader>().DownloadFile(path, isSceneAssetBundle));
    }

    IEnumerator DownloadFile(string path, bool isSceneAssetBundle = false)
    {
        var request = new UnityWebRequest("https://content.dropboxapi.com/2/files/download", "POST");

        request.SetRequestHeader("Authorization", "Bearer ei0yF_QKvGAAAAAAAAAAgEJp_wbL978p9dzsxDimsmAR1va-MKnOA2tQ6QPbQE8d");
        // request.SetRequestHeader("Content-Type", "text/plain");
        request.SetRequestHeader("Dropbox-API-Arg", path);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(path);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(null);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //see if the file exists or if its an older version
        /*
        if (File.Exists(Application.persistentDataPath + SimpleJSON.JSONNode.Parse(path)["path"].Value))
        {
            Debug.Log("file already exists");
            yield break;
        }
        */

        string savePath = "";
        yield return request.SendWebRequest();
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

            System.IO.File.WriteAllBytes(savePath, request.downloadHandler.data);
        }

        var res = request.downloadHandler.text;

        if (isSceneAssetBundle)
        {
            Debug.Log(savePath + " Downloaded");
            StartCoroutine(LoadSceneFromAssetBundle(savePath));
        }
    }

    Dictionary<string, AsyncOperation> SceneLoadDict;
    IEnumerator LoadSceneFromAssetBundle(string path)
    {
        SceneLoadDict = new Dictionary<string, AsyncOperation>();
        var ab = AssetBundleManager.loadFromFile(path, 0);

        if (ab != null)
        {
            // Debug.Log("loading asset bundle");

            if (ab.isStreamedSceneAssetBundle)
            {
                //TODO: THIS MAKES THE ASSUMPTION OF 1 Scene per boundle
                string[] scenePaths = ab.GetAllScenePaths();
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);
                SceneManager.LoadScene(sceneName);
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
}


