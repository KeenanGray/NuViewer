using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGravity : MonoBehaviour
{
    public void Toggle(bool toggle)
    {
        GetComponent<Rigidbody>().useGravity = toggle;
    }
}
