using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitVuf : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Vuforia.VuforiaRuntime.Instance.InitVuforia();

    }

}
