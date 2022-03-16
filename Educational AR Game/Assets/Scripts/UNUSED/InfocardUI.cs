using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfocardUI : MonoBehaviour
{
    [SerializeField] private GameObject contentObj;

    [SerializeField] float[] points;

    [SerializeField] private int screens;

    private float stepSize;

    [SerializeField] private ScrollRect scroll;
    private bool lerpH;
    private float targetH;
    [SerializeField] private bool snapInH = true;

    // Start is called before the first frame update
    void Start()
    {
        scroll.inertia = false;

        if (screens > 0)
        {
            points = new float[screens];
            stepSize = 1/(float)(screens-1);

            for (int i = 0; i < screens; i++)
            {
                points[i] = i * stepSize;
            }
        }
        else
        {
            points[0] = 0;
        }
    }

    //void Awake()
    //{
        //contentObj.transform.localPosition = new Vector3(0f, contentObj.transform.localPosition.y, contentObj.transform.localPosition.z);
    //}

    public void ResetContent()
    {
        contentObj.transform.localPosition = new Vector3(0f, contentObj.transform.localPosition.y, contentObj.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpH)
        {
            scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetH, 20*scroll.elasticity*Time.deltaTime);
            if (Mathf.Approximately(scroll.horizontalNormalizedPosition, targetH))
            {
                lerpH = false;
            }
        }
    }

    public void OnDrag()
    {
        Debug.Log("OnDrag() called");
        lerpH = false;
    }

    public void OnDragEnd()
    {
        Debug.Log("OnDragEnd() called");
        if (scroll.horizontal && snapInH)
        {
            targetH = points[FindNearest(scroll.horizontalNormalizedPosition, points)];
            lerpH = true;
        }
    }

    int FindNearest(float f, float[] array)
    {
        float distance = Mathf.Infinity;
        int output = 0;

        for (int index = 0; index < array.Length; index++)
        {
            if (Mathf.Abs(array[index] - f) < distance)
            {
                distance = Mathf.Abs(array[index] - f);
                output = index;
            }
        }
        return output;
    }
}
