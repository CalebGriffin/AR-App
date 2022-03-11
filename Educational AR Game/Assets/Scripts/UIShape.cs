using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIShape : MonoBehaviour
{
    private Vector3 rotToGetTo;

    private float timePassed;

    public float lerpSpeed;

    public float turnSpeed = 7f;

    public int[] facesArray = {-150, -90, -30, 30, 90, 150};

    private int pageNumber = 30;

    private Touch touch;

    private bool isSpinning = false;

    // Start is called before the first frame update
    void Start()
    {
        rotToGetTo = new Vector3(Random.Range(-5, 5),Random.Range(-5, 5),Random.Range(-5, 5));
        rotToGetTo.y += pageNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != Quaternion.Euler(rotToGetTo) && isSpinning == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotToGetTo), lerpSpeed * timePassed);
            timePassed += Time.deltaTime;
        }
        else
        {
            timePassed = 0;
            rotToGetTo = new Vector3(Random.Range(-5, 5),Random.Range(-5, 5),Random.Range(-5, 5));
            rotToGetTo.y += pageNumber;
        }

        if (Input.touches.Length > 0)
        {
            touch = Input.touches[0];
            //if (touch.deltaPosition.x > 50f)
            //{
                //pageNumber -= 60;
                //rotToGetTo.y += pageNumber;
            //}
            //else if (touch.deltaPosition.x < -50f)
            //{
                //pageNumber += 60;
                //rotToGetTo.y += pageNumber;
            //}
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                isSpinning = true;
                transform.RotateAround(transform.position, Vector3.up, -(touch.deltaPosition.x * turnSpeed));
            }
            else
            {
                isSpinning = false;
                pageNumber = facesArray.OrderBy(x => Mathf.Abs((long) x - transform.rotation.y)).First();
                rotToGetTo.y += pageNumber;
            }
        }
        
    }
}
