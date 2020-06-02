#define LOADER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class AssetBundleManager
{
    // A dictionary to hold the AssetBundle references
    static private Dictionary<string, AssetBundleRef> dictAssetBundleRefs;
    static AssetBundleManager()
    {
        dictAssetBundleRefs = new Dictionary<string, AssetBundleRef>();
    }
    // Class with the AssetBundle reference, url and version
    private class AssetBundleRef
    {
        public AssetBundle assetBundle = null;
        public int version;
        public string url;
        public AssetBundleRef(string strUrlIn, int intVersionIn)
        {
            url = strUrlIn;
            version = intVersionIn;
        }
    };
    // Get an AssetBundle
    public static AssetBundle getAssetBundle(string url, int version)
    {
        string keyName = url + version.ToString();
        AssetBundleRef abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
            return abRef.assetBundle;
        else
            return null;
    }

    //load an asset bundle from a file
    public static AssetBundle loadFromFile(string url, int version)
    {
        string keyName = url + version.ToString();
        AssetBundleRef abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
        {
            Debug.LogWarning("already have that asset bundle loaded");
            return null;
        }
        else
        {
            Debug.Log("Loading Asset Bundle From File");
            try
            {
                var assetbundle = AssetBundle.LoadFromFile(url);
                abRef = new AssetBundleRef(url, version);
                abRef.assetBundle = assetbundle;
                dictAssetBundleRefs.Add(keyName, abRef);
                return assetbundle;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }

    // Unload an AssetBundle
    public static void Unload(string url, int version, bool allObjects)
    {
        string keyName = url + version.ToString();
        AssetBundleRef abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
        {
            abRef.assetBundle.Unload(allObjects);
            abRef.assetBundle = null;
            dictAssetBundleRefs.Remove(keyName);
        }
    }
}
