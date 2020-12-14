using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackToImage : MonoBehaviour
{

    [SerializeField]
    public string imageName = "";

    ARTrackedImage trackedImage = null;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackedImage == null)
        {
            foreach (ARTrackedImage arImg in GameObject.FindObjectsOfType<ARTrackedImage>())
            {
                if (arImg.referenceImage.name.Equals(imageName))
                    trackedImage = arImg;
            }
        }
        else
        {
            //try parenting and local position again
            transform.position = trackedImage.transform.position;
        }
    }
}
