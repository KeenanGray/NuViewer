using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class Example
{

    public const int FILTERMODE_ALL = 0;
    public const int FILTERMODE_NAME = 1;
    public const int FILTERMODE_TYPE = 2;

    //[MenuItem("Search/ShowModels")]
    static void SearchModels()
    {
        // Find all assets labelled with 'architecture' :
        string[] guids1 = AssetDatabase.FindAssets("l:architecture", null);

        foreach (string guid1 in guids1)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid1));
        }

        // Find all Texture2Ds that have 'co' in their filename, that are labelled with 'architecture' and are placed in 'MyAwesomeProps' folder
        string[] guids2 = AssetDatabase.FindAssets("t:Model", new[] { "Assets/Models" });

        foreach (string guid2 in guids2)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid2));
        }

    }

    [MenuItem("Search/ShowModels")]
    public static void SetSearchString()
    {
        Type projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
        EditorWindow window = UnityEditor.EditorWindow.GetWindow(projectBrowserType);

        // UnityEditor.ProjectBrowser.SetSearch(string searchString)
        MethodInfo setSearchMethodInfo = projectBrowserType.GetMethod("SetSearch", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
            null, new Type[] { typeof(string) }, null);

        setSearchMethodInfo.Invoke(window, new object[] { "t:model" });
    }
}