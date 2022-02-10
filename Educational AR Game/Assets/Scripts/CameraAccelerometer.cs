using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAccelerometer : MonoBehaviour
{
    Vector3 prevTilt;

    Vector3 currentTilt;

    Vector3 deltaTilt;

    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tilt = Input.acceleration;

        tilt = Quaternion.Euler(0f, 0f, 0f) * tilt;
        tilt.z = -tilt.z;

        rigid.AddForce(tilt * 2f);

        //currentTilt = Input.acceleration + new Vector3(0f, 90f, 0f);

        //currentTilt.z = -currentTilt.z;

        //deltaTilt = currentTilt - prevTilt;

        //prevTilt = currentTilt;

        //transform.Rotate(deltaTilt * 90f);
    }
}
