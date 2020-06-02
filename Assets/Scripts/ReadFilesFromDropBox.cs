using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;
using UnityEngine.UI;
using Vuforia;

public class ReadFilesFromDropBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("https://api.dropboxapi.com/2/files/list_folder"));
    }


    IEnumerator GetRequest(string uri)
    {
        WWWForm data = new WWWForm();
        data.AddField("path", "");
        //   data.AddField("include_media_info", "false");
        //   data.AddField("include_deleted", "false");
        //   data.AddField("include_has_explicit_shared_numbers", "false");
        //   data.AddField("include_non_downloadable_files", "true");
#if UNITY_IOS || UNITY_EDITOR
        StartCoroutine(Post("https://api.dropboxapi.com/2/files/list_folder", "{\"path\":\"/ios\"}"));
#elif UNITY_ANDROID
        StartCoroutine(Post("https://api.dropboxapi.com/2/files/list_folder", "{\"path\":\"/android\"}"));
#endif
        yield break;
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Authorization", "Bearer ei0yF_QKvGAAAAAAAAAAgEJp_wbL978p9dzsxDimsmAR1va-MKnOA2tQ6QPbQE8d");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        //Debug.Log("Status Code: " + request.responseCode);
        var res = request.downloadHandler.text;
        // Debug.Log(res);
        //parse the json
        var N = JSON.Parse(res);
        var entries = N["entries"].Values;
        foreach (JSONNode jsn in entries)
        {
            Debug.Log(jsn.ToString());
            //if the file is a unity asset bundle, create a button for it
            if (!jsn["name"].Value.Contains(".dat") && !jsn["name"].Value.Contains(".xml"))
            {
                var prefab = Resources.Load("LoadSceneButton") as GameObject;
                var go = Instantiate(prefab, GameObject.Find("Content").transform);
                go.GetComponent<SceneButton>().ui_text.text = jsn["name"].Value;
                go.GetComponent<SceneButton>().ui_button.onClick.AddListener(delegate { StartCoroutine(OpenSceneFromAssetBundle(go.GetComponent<Button>(), jsn["path_display"])); });
            }
            else
            {
                //print out the names of additonal unrecognized files
                //and download them
                AssetBundleDownloader.DownloadFileFromDropBox(jsn["path_display"].Value);
            }
        }
    }

    /*
      handler for downloading and loading an asset bundle when a button is clicked.
  */
    IEnumerator OpenSceneFromAssetBundle(Button btn, string name)
    {
        GameObject.FindObjectOfType<VuforiaBehaviour>().enabled = true;
        Vuforia.VuforiaRuntime.Instance.InitVuforia();
        ///load the datasets
        AssetBundleDownloader.DownloadFileFromDropBox(name, true);
        //disable the button for 1 minute
        btn.interactable = false;
        yield return new WaitForSeconds(1.0f);
        btn.interactable = true;
        yield break;
    }
}
/* json attributes from dropbox
    public string name;
    public string path_lower;
    public string path_display;
    public string id;
    public string client_modified;
    public string server_modified;
    public string rev;
    public int size;
    public bool is_downloadable;
    public string content_hash;
*/

