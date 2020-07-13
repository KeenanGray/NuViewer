using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEditor.SceneManagement;

public class FilterFiles
{
    public const int FILTERMODE_ALL = 0;
    public const int FILTERMODE_NAME = 1;
    public const int FILTERMODE_TYPE = 2;


    [MenuItem("Search/Show Models")]
    public static void SetSearchFilterInProject()
    {
        var searchString = "t:model";

        EditorWindow window = GetProjectWindow();

        // UnityEditor.ProjectBrowser.SetSearch(string searchString)
        MethodInfo setSearchMethodInfo = window.GetType().GetMethod("SetSearch", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
            null, new Type[] { typeof(string) }, null);

        setSearchMethodInfo.Invoke(window, new object[] { searchString });
    }

    //Returns project window
    private static EditorWindow GetProjectWindow()
    {
        EditorWindow[] windows = (EditorWindow[])Resources.FindObjectsOfTypeAll(typeof(EditorWindow));
        EditorWindow projectWindow = null;

        foreach (EditorWindow window in windows)
        {
            if (window.GetType().ToString() == "UnityEditor.ProjectBrowser")
            {
                projectWindow = window;
                break;
            }
        }

        return projectWindow;
    }



    public static void SetSearchFilterInHeirarchy()
    {

        SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
        SearchableEditorWindow hierarchy = null;

        foreach (SearchableEditorWindow window in windows)
        {

            if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow")
            {
                hierarchy = window;
                break;
            }
            else
            {
                // Debug.Log(window.GetType().ToString());
            }
        }

        if (hierarchy == null)
            return;

        MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
        object[] parameters = new object[] { "t:model", 0, false, false };

        setSearchType.Invoke(hierarchy, parameters);
    }


}