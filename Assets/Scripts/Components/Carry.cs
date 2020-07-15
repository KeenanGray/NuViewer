using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carry : MonoBehaviour
{
    public float CarryDistance;
    public bool ReturnPositionOnDrop = false;
    Transform oldParent;
    Vector3 oldPosition;
    bool pickedUp;
    public void toggleCarry()
    {
        if (transform.parent != Camera.main.transform && !pickedUp)
        {
            PickUp();
            return;
        }
        else if (pickedUp)
        {
            Drop();
            return;
        }

    }

    public void PickUp()
    {
        Debug.Log("pick");
        oldPosition = transform.position;
        oldParent = transform.parent;
        pickedUp = true;
        transform.SetParent(Camera.main.transform);
        transform.position = Camera.main.transform.position + (Camera.main.transform.forward * CarryDistance);
    }

    public void Drop()
    {
        pickedUp = false;
        Debug.Log("Drop");
        transform.parent = oldParent;

        if (ReturnPositionOnDrop)
            transform.position = oldPosition;
    }
}
