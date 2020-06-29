using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoolReference hasGameStarted;

    void Awake()
    {
        hasGameStarted.Value = false;
    }
    public void GameStarted()
    {
        if (!hasGameStarted.Value)
        {
            hasGameStarted.Value = true;
            //turn off the green indicator thing
            GameObject.Find("DefaultIndicator").GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        else
        {
            Debug.Log("Already Started");
        }
    }

    private void OnDestroy()
    {
        hasGameStarted.Value = false;
    }
}
