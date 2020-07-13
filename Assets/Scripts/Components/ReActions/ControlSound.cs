using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ControlSound : MonoBehaviour
{
    AudioSource my_audio;

    void Awake()
    {
        my_audio = GetComponent<AudioSource>();
    }

    public void Play()
    {
        my_audio.Play();
    }

    public void Stop()
    {
        my_audio.Stop();
    }

    public void Pause()
    {
        my_audio.Pause();
    }
    public void UnPause()
    {
        my_audio.UnPause();
    }

    public void SetLoop(bool onoff)
    {
        my_audio.loop = onoff;
    }
    public void IncreaseVolume(int volume)
    {
        my_audio.volume += volume;
    }
    public void DecreaseVolume(int volume)
    {
        my_audio.volume -= volume;
    }

    public void ToggleAudio()
    {
        Debug.Log("toggle audio");
        if (my_audio.isPlaying)
        {
            Debug.Log("stopping");
            my_audio.Stop();
        }
        else
        {
            Debug.Log("playing");
            my_audio.Play();
        }
    }
    public void TogglePause()
    {
        if (my_audio.isPlaying)
        {
            my_audio.Pause();
        }
        else if(!my_audio.isPlaying)
        {
            my_audio.UnPause();
        }
    }

    public void PlayForSeconds(float seconds)
    {
        StartCoroutine(co_PlayForSeconds(seconds));
    }

    IEnumerator co_PlayForSeconds(float seconds)
    {
        my_audio.Play();
        yield return new WaitForSeconds(1.0f);
        my_audio.Stop();
        yield break;
    }

    public void FadeIn()
    {

    }
}
