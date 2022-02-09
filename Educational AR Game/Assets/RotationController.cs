using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private GameObject prefabHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(-prefabHolder.transform.rotation.x, transform.rotation.y, -prefabHolder.transform.rotation.z);
    }
}
