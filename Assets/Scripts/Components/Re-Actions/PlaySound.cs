using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    AudioSource audio;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    public void Play()
    {

    }
    public void Stop()
    {

    }
    public void Pause()
    {

    }
    public void SetLoop(bool onoff)
    {

    }
}
