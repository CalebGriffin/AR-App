using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private GameObject prefabHolder; // Reference to the parent object of the shape

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // reverses the rotation of the parent object in the x and z axis so that the shape always appears to be upright
        transform.rotation = Quaternion.Euler(-prefabHolder.transform.rotation.x, transform.rotation.y, -prefabHolder.transform.rotation.z);
    }
}
