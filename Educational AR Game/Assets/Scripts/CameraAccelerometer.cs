using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will add a force to the shape on the menu based on the rotation of the phone
public class CameraAccelerometer : MonoBehaviour
{
    // UNUSED
    Vector3 prevTilt;
    Vector3 currentTilt;
    Vector3 deltaTilt;

    private Rigidbody rigid; // The rigidbody will be used to give the object force and mass

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component and assign it to the rigidbody variable
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the rotation of the device that the game is running on
        Vector3 tilt = Input.acceleration;

        // Set the rotation to a Quaternion
        tilt = Quaternion.Euler(0f, 0f, 0f) * tilt;
        // Reverse the z of the tilt
        tilt.z = -tilt.z;

        // Apply the force to the object
        rigid.AddForce(tilt * 10f);
    }

    // Toggles whether the object will be affected by the forces or not, called by the button in the settings
    public void PhysicsToggle()
    {
        rigid.isKinematic = !rigid.isKinematic;
    }
}
