using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 offset;

    GameObject my_object;

    void Awake(){
         my_object = Instantiate(objectToSpawn);
        // my_object.hideFlags = HideFlags.HideInHierarchy;
         foreach(Behaviour c in my_object.GetComponents<Behaviour>()){
             c.enabled=false;
         }
          foreach(MonoBehaviour mb in my_object.GetComponents<MonoBehaviour>()){
             mb.StopAllCoroutines();
         }
         
    }

    public void Activate()
    {
        var go = Instantiate(my_object, transform.parent);
        
        // go.transform.position = new Vector3(0, 0, 0);
        go.transform.localPosition = new Vector3(0, 0, 0) + offset;
        //   go.transform.localScale = new Vector3(1, 1, 1);

        go.transform.localPosition += offset;

        foreach(Behaviour c in go.GetComponents<Behaviour>()){
             c.enabled=true;
         }
    }
}
