using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class OnCollision : MonoBehaviour
{
    public UnityEvent onCollisionEnter;
    public string[] tags;

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        bool correctObject = false;
        foreach (string tag in tags)
        {
            if (other.gameObject.CompareTag(tag))
                correctObject = true;
        }

        if (correctObject || tags.Length <= 0)
            onCollisionEnter.Invoke();

    }


}
