using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTap : MonoBehaviour
{
    public UnityEvent onObjectTapped;
    public UnityEvent onTapEnded;

    void Tapped()
    {
        Debug.Log("tapped");
        onObjectTapped.Invoke();
        StartCoroutine("EndTap");
    }

    IEnumerator EndTap()
    {
        Debug.Log("end tap");
        yield return new WaitForSeconds(.25f);
        onTapEnded.Invoke();
    }
}
