﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour
{
    // public float delay;

    void Update()
    {
    }

    public void Activate(float delay)
    {
        StartCoroutine(DestroyAfterTime(delay));
    }

    IEnumerator DestroyAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}
