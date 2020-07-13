using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : MonoBehaviour
{
    GameObject go = null;

   void Enable()
   {
       go.SetActive(true);
   }

   void Disable()
   {
       go.SetActive(false);
   }
}
