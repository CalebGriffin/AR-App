using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    public Text testText;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.transform.localScale = Vector3.zero;
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(newPrefab.name, newPrefab);
            //newPrefab.SetActive(false);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            AddImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            testText.text = "Nothing";
        }
    }

    private void AddImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        testText.text = name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.transform.rotation = rotation;
        prefab.transform.localScale = new Vector3(trackedImage.size.x * 2, trackedImage.size.x * 2, trackedImage.size.y * 2);
        prefab.SetActive(true);
        prefab.GetComponent<PrefabHolder>().Enable();
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        testText.text = name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.transform.rotation = rotation;
        prefab.transform.localScale = new Vector3(trackedImage.size.x * 2, trackedImage.size.x * 2, trackedImage.size.y * 2);
        if (prefab.activeSelf == false)
        {
            prefab.SetActive(true);
        }

        //foreach(GameObject go in spawnedPrefabs.Values)
        //{
            //if (go.name != name)
            //{
                //go.SetActive(false);
            //}
        //}
    }
}