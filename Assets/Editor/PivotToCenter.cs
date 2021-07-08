using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PivotModelToCenter
{
    [MenuItem("GameObject/Pivot To Center")]
    public static void PivotToCenter()
    {

        if (Selection.objects.Length != 1)
            return;

        GameObject parent = null;

        var go = Selection.gameObjects[0];

        if (parent == null)
            parent = GameObject.Instantiate(new GameObject());

        Debug.Log(parent.name);

        //cube is at position of object
        if (go.transform.parent != parent)
            parent.transform.position = go.transform.position;

        //else, store the old parent
        var oldParent = go.transform.parent;

        Vector3 centerPoint = new Vector3(0, 0, 0);
        //calculate the center of the mesh based on all children
        foreach (MeshRenderer mr in go.GetComponentsInChildren<MeshRenderer>())
        {
            centerPoint += mr.bounds.center;
        }
        var cnt = go.GetComponentsInChildren<MeshRenderer>().Length;
        if (cnt != 0)
            centerPoint = new Vector3(centerPoint.x / cnt, centerPoint.y / cnt, centerPoint.z / cnt);

        parent.transform.position = centerPoint;
        //parent to center point;
        go.transform.SetParent(parent.transform);

        //cleanup

        //Rename the new parent
        parent.name = go.gameObject.name + "_New";
        //reparent the object to it's original parent
        parent.transform.SetParent(oldParent);

    }
}
