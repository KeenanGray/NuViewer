using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public BoolReference hasGameStarted;
    public GameObject go;

    public void Activate()
    {
        if (hasGameStarted.Value)
        {
            Instantiate(go,transform);
        }
    }
}
