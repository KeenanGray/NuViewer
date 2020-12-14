using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[ExecuteInEditMode]
public class TrackedImageManagerHelper : MonoBehaviour
{

    [SerializeField]
    GameObject HelperObject;

    [SerializeField]
    Shader shader;

    int Images;
    ARTrackedImageManager TrackedImageManager = null;
    GameObject[] helperObjects = null;
    Material[] ImageMaterials;


    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (TrackedImageManager == null)
            TrackedImageManager = GetComponent<ARTrackedImageManager>();

        if (helperObjects == null)
        {
            helperObjects = new GameObject[GameObject.FindGameObjectsWithTag("TrackedImageHelper").Length];
            for (int i = 0; i < helperObjects.Length; i++)
            {
                helperObjects[i]=GameObject.FindGameObjectsWithTag("TrackedImageHelper")[i];
            }
            return;
        }
        if (ImageMaterials == null || ImageMaterials.Length != helperObjects.Length)
        {
            ImageMaterials = new Material[helperObjects.Length];
            InstantiateMaterials();
            return;
        }

        //if we change number of objects in the library, recreaete array
        if (helperObjects.Length != TrackedImageManager.referenceLibrary.count || 
        GameObject.FindGameObjectsWithTag("TrackedImageHelper").Length != TrackedImageManager.referenceLibrary.count
        )
        {
            //recreate the array
            helperObjects = new GameObject[TrackedImageManager.referenceLibrary.count];
            for (int i = 0; i < Mathf.Min(helperObjects.Length ,GameObject.FindGameObjectsWithTag("TrackedImageHelper").Length); i++)
            {
                print(i);
                helperObjects[i]=GameObject.FindGameObjectsWithTag("TrackedImageHelper")[i];
            }
            ImageMaterials = new Material[TrackedImageManager.referenceLibrary.count];
            InstantiateMaterials();

        }

        for (int i = 0; i < TrackedImageManager.referenceLibrary.count; i++)
        {
            if (helperObjects[i] == null)
                helperObjects[i] = Instantiate(HelperObject);

            helperObjects[i].transform.GetChild(0).localScale = new Vector3(TrackedImageManager.referenceLibrary[i].width, TrackedImageManager.referenceLibrary[i].height, 1);
            helperObjects[i].name = TrackedImageManager.referenceLibrary[i].name;
            helperObjects[i].GetComponent<TrackToImage>().imageName = TrackedImageManager.referenceLibrary[i].name;

            if (ImageMaterials[i] != null)
                helperObjects[i].GetComponentInChildren<Renderer>().sharedMaterial = ImageMaterials[i];

        }
        #endif
    }

    void InstantiateMaterials()
    {
        for (int i = 0; i < ImageMaterials.Length; i++)
        {
            ImageMaterials[i] = Instantiate(Resources.Load("ImageTargetMaterial") as Material);
            ImageMaterials[i].SetTexture("_MainTex", TrackedImageManager.referenceLibrary[i].texture);
        }
    }

    void OnEnable()
    {
       
    }
    void OnDisable()
    {
       
    }

}
