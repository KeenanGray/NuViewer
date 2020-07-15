using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDestroyed : MonoBehaviour
{
    public UnityEvent onDestroyed;
    void OnDestroy()
    {
            Debug.Log("Destroyed");
            onDestroyed.Invoke();
    }
}
