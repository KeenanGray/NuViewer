using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnObjectEnabled : MonoBehaviour
{
    public UnityEvent onEnable;
    bool hasObjectLoaded = false;
    void OnEnabled()
    {
       onEnable.Invoke();
    }
}
