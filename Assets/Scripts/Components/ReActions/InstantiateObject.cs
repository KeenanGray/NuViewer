using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject go;
    public Vector3 offset;
    public void Activate()
    {
       var ngo =  Instantiate(go, transform);
        ngo.transform.localPosition = offset;
    }
}
