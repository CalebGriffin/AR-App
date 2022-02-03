using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    [SerializeField] private GameObject objectObj;
    [SerializeField] private GameObject infocardObj;

    // Start is called before the first frame update
    void Start()
    {
        //objectObj.SetActive(false);
        //infocardObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable()
    {
        //objectObj.transform.localScale = Vector3.zero;
        objectObj.SetActive(true);
        objectObj.GetComponent<Object>().AnimateIn();
    }

    void OnDisable()
    {
        //objectObj.SetActive(false);
        //infocardObj.SetActive(false);
    }
}
