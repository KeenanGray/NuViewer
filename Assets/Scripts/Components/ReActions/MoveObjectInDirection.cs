using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectInDirection : MonoBehaviour
{
    public Vector3 WorldDirection;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(WorldDirection * speed);
    }


    public void increaseSpeed(float amt)
    {
        speed += amt;
    }
     public void descreaseSpeed(float amt)
    {
        speed -= amt;
    }
      public void setSpeed(float amt)
    {
        speed = amt;
    }
}
