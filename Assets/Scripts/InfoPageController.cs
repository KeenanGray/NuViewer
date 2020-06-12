using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPageController : MonoBehaviour
{
    public void HideInfoScreen(bool value)
    {
        GameObject.Find("InfoScreen").GetComponent<Canvas>().enabled=!value;
    }
}
