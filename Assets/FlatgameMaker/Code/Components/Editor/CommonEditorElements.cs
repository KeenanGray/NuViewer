using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CommonEditorElements : Editor
{
    public static void MakePlayerButton(GameObject selected)
    {
        if (GUILayout.Button("Make Player"))
        {
            Area area = AreaManager.Instance.FindAreaOf(selected.transform);
            if (area != null)
            {
                Player player = area.AreaPlayer;
                int artIndex = selected.transform.GetSiblingIndex();
                if (player.transform.childCount > 0)
                {
                    var tmpTransform = player.transform.parent;

                    foreach (Transform t in player.transform.GetChildren())
                    {
                        t.parent = tmpTransform.transform;
                    }
                }
                player.transform.SetSiblingIndex(artIndex);
                player.transform.parent = selected.transform.parent;
                selected.transform.parent = player.transform;
            }
        }
    }
}
