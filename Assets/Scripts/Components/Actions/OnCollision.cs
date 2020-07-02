using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class OnCollision : MonoBehaviour
{
    public UnityEvent onCollisionEnter;
    public string[] tags;
    bool hasObjectLoaded = false;


    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindObjectOfType<GameManager>().hasGameStarted.Value && !hasObjectLoaded)
        {
            hasObjectLoaded = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (hasObjectLoaded)
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
}
