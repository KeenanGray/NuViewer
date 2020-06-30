using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;
using UnityEngine.UI;
using Vuforia;
using System;

public class ReadFilesFromDropBox : MonoBehaviour
{
    string bearer;
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

        //first download root folder files, xml and .dat - platform independent
        StartCoroutine(Post("https://api.dropboxapi.com/2/files/list_folder", "{\"path\":\"\"}"));

#if UNITY_IOS 
        StartCoroutine(Post("https://api.dropboxapi.com/2/files/list_folder", "{\"path\":\"/ios\"}"));
#elif UNITY_ANDROID || UNITY_EDITOR
        StartCoroutine(Post("https://api.dropboxapi.com/2/files/list_folder", "{\"path\":\"/android\"}"));
#endif
        yield break;
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");

        bearer = AssetBundleDownloader.getBearerString();
        request.SetRequestHeader("Authorization", "Bearer " + bearer);

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

        //Create a dictionary of the latest files.
        var FilesFromDropbox = CreateFilesDictionary(entries);

        foreach (JSONNode jsn in FilesFromDropbox.Values)
        {
            //Debug.Log(jsn.ToString());
            //if the file is a unity asset bundle, create a button for it
            if (!jsn["name"].Value.Contains(".dat") && !jsn["name"].Value.Contains(".xml") && jsn[".tag"].Value != "folder")
            {
                var prefab = Resources.Load("LoadSceneButton") as GameObject;
                var go = Instantiate(prefab, GameObject.Find("ProjectContent").transform);
                go.GetComponent<SceneButton>().ui_text.text = jsn["name"].Value.Split(' ')[jsn["name"].Value.Split(' ').Length - 1];
                go.GetComponent<SceneButton>().ui_button.onClick.AddListener(delegate { StartCoroutine(OpenSceneFromAssetBundle(go.GetComponent<Button>(), jsn["path_display"])); });
            }
            else //non "scene asset bundle" files are downloaded [.xml and .dat files required for Vuforia]
            {
                if (jsn[".tag"].Value != "folder")
                {
                    //print out the names of additonal unrecognized files
                    //and download them
                    AssetBundleDownloader.DownloadFileFromDropBox(jsn["path_display"].Value, false);
                }
            }
        }
    }

    Dictionary<string, JSONNode> CreateFilesDictionary(JSONNode.ValueEnumerator entries)
    {
        var result = new Dictionary<string, JSONNode>();

        foreach (JSONNode jsn in entries)
        {
            var name = jsn["name"].Value.Split(' ')[jsn["name"].Value.Split(' ').Length - 1];
            //add each result to the dictionary

            if (!result.ContainsKey(name))
            {
                result.Add(name, jsn);
            }
            else
            {
                //if result already contains key, compare the modified dates in UTC
                var Date1 = result[name]["server_modified"].Value;
                var Date2 = jsn["server_modified"].Value;

                if (DateTime.Parse(Date1) < DateTime.Parse(Date2))
                {
                    result[name] = jsn;
                }
            }
        }

        return result;
    }

    /*
      handler for downloading and loading an asset bundle when a button is clicked.
  */
    IEnumerator OpenSceneFromAssetBundle(Button btn, string name)
    {
        ///Downloads the asset bundle and loads it from filestream
        AssetBundleDownloader.DownloadFileFromDropBox(name, true);
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
