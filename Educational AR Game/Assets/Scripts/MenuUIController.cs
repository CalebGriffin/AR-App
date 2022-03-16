using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    private ScreenOrientation currentOrientation; // Current screen orientation used to lock the screen orientation and check which way the device is oriented
    private bool screenLocked = false; // Used when the user toggles the screen rotation lock

    [SerializeField] private GameObject xImage; // Used to show that the screen rotation lock is enabled

    [SerializeField] private GameObject leftPlane; // GameObject used for the left bounds of where the shapes can hit
    [SerializeField] private GameObject rightPlane; // GameObject used for the right bounds of where the shapes can hit
    [SerializeField] private GameObject settingsPanel; // Used to show and hide the settings panel
    [SerializeField] private GameObject[] menuObjects; // The shape objects that are on the menu, used to be able to enable and disable the physics on all of them

    // Start is called before the first frame update
    void Start()
    {
        // Find all of the shape objects in the scene
        menuObjects = GameObject.FindGameObjectsWithTag("Object");
    }

    // Update is called once per frame
    void Update()
    {
        // If the screenlock is enabled then set the screens orientation to the current orientation
        if (screenLocked)
        {
            Screen.orientation = currentOrientation;
        }
    }

    void FixedUpdate()
    {
        // Set the locations of the right and left bounds based on whether the screen rotation is landscape or portrait
        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            leftPlane.transform.localPosition = new Vector3(-3f, 0f, 0f);
            rightPlane.transform.localPosition = new Vector3(3f, 0f, 0f);
        }
        else
        {
            leftPlane.transform.localPosition = new Vector3(-1.5f, 0f, 0f);
            rightPlane.transform.localPosition = new Vector3(1.5f, 0f, 0f);
        }
    }

    // Called by a button on the menu
    public void OrientationLock()
    {
        // Toggle the screen lock boolean and set the image to show that the rotation is locked
        if (!screenLocked)
        {
            xImage.SetActive(true);
            currentOrientation = Screen.orientation;
            screenLocked = true;
        }
        else
        {
            xImage.SetActive(false);
            screenLocked = false;
        }
    }

    // Toggle the visibility of the settings panel, called when the settings button is pressed
    public void SettingsButton()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    
    // Toggle the physics on the objects in the background of the menu, called by a button in the settings menu
    public void GyroToggle(bool value)
    {
        foreach (GameObject obj in menuObjects)
        {
            obj.GetComponent<CameraAccelerometer>().PhysicsToggle();
        }
    }
}
