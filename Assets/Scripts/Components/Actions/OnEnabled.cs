using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnabled : MonoBehaviour
{
    public UnityEvent onEnable;
    void OnEnable()
    {
            Debug.Log("Enabled");
            onEnable.Invoke();
    }
}
