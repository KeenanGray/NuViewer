using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayButtonPress : MonoBehaviour
{
    public void Delay(float time)
    {
        StartCoroutine(DelayTimer(time));
    }

    IEnumerator DelayTimer(float time)
    {
        GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(time);
        GetComponent<Button>().interactable = true;
        yield break;

    }

}