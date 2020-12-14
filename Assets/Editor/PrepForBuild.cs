using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEditor.SceneManagement;
using Vuforia;

public class PrepForBuild
{
    [MenuItem("Tools/Prepare For Build")]
    public static void PrepSceneForBuild()
    {
        var n = 0;
        foreach (Transform t in Resources.FindObjectsOfTypeAll(typeof(Transform)))
        {
            var tob = t.GetComponent<Vuforia.TurnOffBehaviour>();
            if (tob != null)
            {
                n++;
                Debug.Log("Updated " + n + " image target(s)");
                t.gameObject.AddComponent<TurnOffBehaviorCustom>();
                GameObject.DestroyImmediate(tob, true);
            }
        }
        n = 0;
        foreach (TurnOffBehaviorCustom t in GameObject.FindObjectsOfType<TurnOffBehaviorCustom>())
        {
            //make the texture saved to the renderer "readable" so it can be saved later
            SetTextureImporterFormat((Texture2D)t.GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex"), true);

            n++;
        }
        AssetDatabase.Refresh();

        Debug.Log(n + " Calibrated Image Targets in Scene");
    }

    public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
    {
        if (null == texture) return;

        string assetPath = AssetDatabase.GetAssetPath(texture);
        var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (tImporter != null)
        {
            tImporter.textureType = TextureImporterType.Default;

            tImporter.isReadable = isReadable;

            AssetDatabase.ImportAsset(assetPath);
        }
    }
}