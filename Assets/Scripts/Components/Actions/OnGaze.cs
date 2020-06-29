using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnGaze : MonoBehaviour
{
    public UnityEvent onGazedBegin;
    public UnityEvent whileGazedOn;
    public UnityEvent onGazeEnded;


    void Awake()
    {
        GazeSelect.GazeChangedCallback += GazedBegin;
        GazeSelect.GazeChangedCallback += GazeEnded;
    }

    void GazedBegin(Collider c)
    {
        if (c.Equals(GetComponent<Collider>()))
        {
            Debug.Log("Gaze Began");
            onGazedBegin.Invoke();
        }
    }

    void Gazed()
    {
        whileGazedOn.Invoke();
    }

    void GazeEnded(Collider c)
    {
        if (!c.Equals(GetComponent<Collider>()))
        {
            Debug.Log("Gaze Ended");
            onGazeEnded.Invoke();
        }
    }


//remove the callbacks when the object is destroyed
    void OnDestroy()
    {
        GazeSelect.GazeChangedCallback -= GazedBegin;
        GazeSelect.GazeChangedCallback -= GazeEnded;

    }
}
