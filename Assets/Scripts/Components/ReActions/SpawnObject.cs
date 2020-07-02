using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 offset;

    public void Activate()
    {
        var go = Instantiate(objectToSpawn, transform.parent);
        // go.transform.position = new Vector3(0, 0, 0);
        go.transform.localPosition = new Vector3(0, 0, 0) + offset;
        //   go.transform.localScale = new Vector3(1, 1, 1);

        go.transform.localPosition += offset;
    }
}
