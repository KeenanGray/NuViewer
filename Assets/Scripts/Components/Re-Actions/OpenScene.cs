using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScene : MonoBehaviour
{
    public StringReference abName;

    public void OpenSceneByName(string name)
    {
        if (abName.Value != null)
            AssetBundleManager.Unload(abName.Value, 0, true);


        UnityEngine.SceneManagement.SceneManager.LoadScene(name);


    }
}
