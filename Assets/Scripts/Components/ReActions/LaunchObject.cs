using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LaunchObject : MonoBehaviour
{
    public Vector3 directionAndForce;
    public bool random;

    public Vector3 randomMin;
    public Vector3 randomMax;
    Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Activate()
    {
        if (random)
        {
            rb.AddForce(new Vector3(Random.Range(randomMin.x, randomMax.x),
          Random.Range(randomMin.y, randomMax.y),
          Random.Range(randomMin.z, randomMax.z)));
        }
        else
        {
            rb.AddForce(directionAndForce);
        }
    }
}
