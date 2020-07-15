using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnWaitForSeconds : MonoBehaviour
{
    public UnityEvent onWaitComplete;

    public void StartTimer(float delay)
    {
        StartCoroutine(WaitForSeconds(delay));
    }

    IEnumerator WaitForSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        onWaitComplete.Invoke();
        yield break;
    }
}
