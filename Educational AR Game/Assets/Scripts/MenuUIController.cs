using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    private ScreenOrientation currentOrientation;
    private bool screenLocked = false;

    [SerializeField] private GameObject xImage;

    [SerializeField] private GameObject leftPlane;
    [SerializeField] private GameObject rightPlane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (screenLocked)
        {
            Screen.orientation = currentOrientation;
        }
    }

    void FixedUpdate()
    {
        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            leftPlane.transform.position = new Vector3(-3f, 0f, 0f);
            rightPlane.transform.position = new Vector3(3f, 0f, 0f);
        }
        else
        {
            leftPlane.transform.position = new Vector3(-1.5f, 0f, 0f);
            rightPlane.transform.position = new Vector3(1.5f, 0f, 0f);

        }
    }

    public void OrientationLock()
    {
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
}
