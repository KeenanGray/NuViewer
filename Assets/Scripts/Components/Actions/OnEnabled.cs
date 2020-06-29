using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnabled : MonoBehaviour
{
    public UnityEvent onEnable;
    bool hasObjectLoaded = false;
    void Update()
    {
        if (GameObject.FindObjectOfType<GameManager>().hasGameStarted.Value && !hasObjectLoaded)
        {
            Debug.Log("Enabled");
            onEnable.Invoke();
            hasObjectLoaded = true;
        }
    }
}
