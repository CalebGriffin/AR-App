using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    [SerializeField] private GameObject objectObj; // Reference to the shape object so this object can set is active
    [SerializeField] private GameObject infocardObj; // Reference to the infocard object // UNUSED

    // Start is called before the first frame update
    void Start()
    {
        // UNUSED
        //objectObj.SetActive(false);
        //infocardObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When this object is enabled, enable the shape object and call it's starting animation
    public void Enable()
    {
        //objectObj.transform.localScale = Vector3.zero;
        objectObj.SetActive(true);
        objectObj.GetComponent<Object>().AnimateIn();
    }

    // UNUSED
    void OnDisable()
    {
        //objectObj.SetActive(false);
        //infocardObj.SetActive(false);
    }
}
