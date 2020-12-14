using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;

public class DatasetLoader : MonoBehaviour
{

    // Model is the GameObject to be augmented
    public GameObject Model;

    // Use this for initialization
    void Start()
    {
        // Registering call back to know when Vuforia is ready
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    // This function is called when vuforia gives the started callback
    void OnVuforiaStarted()
    {
        // Request an ImageTracker instance from the TrackerManager.
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        objectTracker.Stop();
        IEnumerable<DataSet> dataSetList = objectTracker.GetActiveDataSets();
        foreach (DataSet set in dataSetList.ToList())
        {
            objectTracker.DeactivateDataSet(set);
        }

        GetFileByPlatform("VuforiaMars_Images.xml");
        GetFileByPlatform("AR-Games-2020.xml");
    }

    void GetFileByPlatform(string fileName)
    {

        // The 'path' string determines the location of xml file
        // For convinence the RealTime.xml is placed in the StreamingAssets folder
        // This file can be downloaded and the relative path will be used accordingly

        string path = "";
#if UNITY_IOS && !UNITY_EDITOR
		path = Application.persistentDataPath + "/"+fileName;
#elif UNITY_ANDROID && !UNITY_EDITOR
///storage/emulated/0/Android/data/com.KeenanGray.VuforiaLoader/files/AR-Games-2020.xml 
		path = Application.persistentDataPath + "/"+fileName;
#else
        path = Application.persistentDataPath + "/" + fileName;
#endif

        bool status = LoadDataSet(path, VuforiaUnity.StorageType.STORAGE_ABSOLUTE);

        if (status)
        {
            Debug.Log("Dataset Loaded");
        }
        else
        {
            Debug.Log("Dataset Load Failed " + path);
        }
    }

    // Load and activate a data set at the given path.
    private bool LoadDataSet(string dataSetPath, VuforiaUnity.StorageType storageType)
    {
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        // Check if the data set exists at the given path.
        if (!DataSet.Exists(dataSetPath, storageType))
        {
            Debug.LogError("Data set " + dataSetPath + " does not exist.");
            return false;
        }

        // Create a new empty data set.
        DataSet dataSet = objectTracker.CreateDataSet();

        // Load the data set from the given path.
        if (!dataSet.Load(dataSetPath, storageType))
        {
            Debug.LogError("Failed to load data set " + dataSetPath + ".");
            return false;
        }

        // (Optional) Activate the data set.
        objectTracker.ActivateDataSet(dataSet);
        objectTracker.Start();


        return true;
    }

}