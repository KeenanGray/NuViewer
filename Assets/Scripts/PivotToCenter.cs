using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PivotToCenter : MonoBehaviour
{
    GameObject parent = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var cube = Resources.Load("Cube") as GameObject;
        if (parent == null)
            parent = Instantiate(cube);

        Debug.Log(parent.name);

        //cube is at position of object
        if (transform.parent != parent)
            parent.transform.position = transform.position;

        //else, store the old parent
        var oldParent = transform.parent;

        Vector3 centerPoint = new Vector3(0, 0, 0);
        //calculate the center of the mesh based on all children
        foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            centerPoint += mr.bounds.center;
        }
        var cnt = gameObject.GetComponentsInChildren<MeshRenderer>().Length;
        if (cnt != 0)
            centerPoint = new Vector3(centerPoint.x / cnt, centerPoint.y / cnt, centerPoint.z / cnt);


        parent.transform.position = centerPoint;
        //parent to center point;
        transform.SetParent(parent.transform);

        //cleanup

        //Rename the new parent
        parent.name = gameObject.name + "_New";
        //reparent the object to it's original parent
        parent.transform.SetParent(oldParent);

        //disable this script;
        enabled=false;
    }
}
