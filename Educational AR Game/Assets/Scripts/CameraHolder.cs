using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private GameObject cameraObj; // This object will copy the location of the Camera in the scene

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Every frame, set the rotation and position of this object to the Camera's rotation and position
    // This is so that Prefabs can find the position of the Camera in real space
    void Update()
    {
        transform.position = cameraObj.transform.position;
        transform.localRotation = cameraObj.transform.localRotation;
    }

    // Returns the Camera in the scene
    public Camera GetCamera()
    {
        return cameraObj.GetComponent<Camera>();
    }
}
