using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGyro : MonoBehaviour
{
    public Gyroscope gyroscope;

    // Start is called before the first frame update
    void Start()
    {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
        //Physics.gravity = gyroscope.attitude.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        GyroModifyCamera();
    }

    void Awake()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void GyroModifyCamera()
    {
        Vector3 tilt = gyroscope.attitude.eulerAngles + new Vector3(90f, -90f, 90f);
        transform.rotation = GyroToUnity(Quaternion.Euler(tilt));
        //transform.rotation = Quaternion.Euler(tilt);
        //transform.Rotate(GyroToUnity(gyroscope.attitude).eulerAngles);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
