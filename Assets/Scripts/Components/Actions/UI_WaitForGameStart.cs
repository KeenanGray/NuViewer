using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_WaitForGameStart : MonoBehaviour
{
    BoolReference hasGameStarted;

    private void Start()
    {
        hasGameStarted=GameObject.FindObjectOfType<GameManager>().hasGameStarted;
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            GetComponent<Canvas>().enabled = hasGameStarted.Value;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            Debug.LogError("no canvas attached to gameobject ");
        }
    }
}
