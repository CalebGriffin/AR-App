using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

// If no component is found then one will be added automatically, helping to prevent reference errors
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs; // An array of prefabs that will be spawned on the detected images

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>(); // Dictionary of names of each shape object with the prefab for that shape so that they can be spawned by string value
    private ARTrackedImageManager trackedImageManager; // Reference to the ARTrackedImageManager component which is used to detect when an image is seen by the camera

    public Text testText; // Text used for testing to show which image has been detected by the camera

    // Awake is called on the first frame update
    private void Awake()
    {
        // Find the ARTrackedImageManager component
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        // Spawn all of the prefabs and add them to the dictionary with their name so that they can be referenced when a shape is spawned
        foreach(GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.transform.localScale = Vector3.zero;
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(newPrefab.name, newPrefab);
            //newPrefab.SetActive(false);
        }
    }

    // When the object is enabled, subscribe to the trackedImagesChanged event using the ImageChanged Function
    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    // When the object is disabled, unsubscribe to the trackedImagesChanged event using the ImageChanged Function
    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    // When an images detected state has changed, call the function that will update the shapes
    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // For every image that has been added, call the AddImage function
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            AddImage(trackedImage);
        }
        // For every image that has been updated, call the UpdateImage function
        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        // For every image that has been deleted, remove the shape object from the scene and update the test text
        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            testText.text = "Nothing";
        }
    }

    // Called on the first frame that a new tracking image is detected
    private void AddImage(ARTrackedImage trackedImage)
    {
        // Get the position and rotation of the tracked image
        string name = trackedImage.referenceImage.name;
        testText.text = name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;

        // Set the position and rotation of the shape object to the position and rotation of the tracked image
        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.transform.rotation = rotation;
        // Set the scale so that it appears the same size as the image
        prefab.transform.localScale = new Vector3(trackedImage.size.x * 2, trackedImage.size.x * 2, trackedImage.size.y * 2);
        // Enable the shape object
        prefab.SetActive(true);
        // Call the Enable function on the PrefabHolder that will start the shape animation
        prefab.GetComponent<PrefabHolder>().Enable();
    }

    // Called on every frame when a tracking image is detected
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        // Get the position and rotation of the tracked image
        string name = trackedImage.referenceImage.name;
        testText.text = name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;

        // Set the position and rotation of the shape object to the position and rotation of the tracked image
        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.transform.rotation = rotation;
        // Set the scale so that it appears the same size as the image
        prefab.transform.localScale = new Vector3(trackedImage.size.x * 2, trackedImage.size.x * 2, trackedImage.size.y * 2);
        // If the object is disabled then enable it
        if (prefab.activeSelf == false)
        {
            prefab.SetActive(true);
        }

        // UNUSED
        //foreach(GameObject go in spawnedPrefabs.Values)
        //{
            //if (go.name != name)
            //{
                //go.SetActive(false);
            //}
        //}
    }
}