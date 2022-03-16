using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// If no component is found then one will be added automatically, helping to prevent reference errors
[RequireComponent(typeof(Animator))]
public class Object : MonoBehaviour
{
    [SerializeField] private GameObject prefabHolder; // Reference to the parent object of the shape
    [SerializeField] private GameObject infoCard; // Reference to the infocard object
    [SerializeField] private GameObject arCamera; // UNUSED
    [SerializeField] private GameObject planeObj; // Reference to the plane object

    public float multiplier; // Float used to control how high above the parent object the shape is

    [SerializeField] private float sensitivity = 0.5f; // Float to control the sensitivity of the touch spinning

    [SerializeField] private float spinningSpeed = 7f; // Float to control how fast the shape spins when not being touched

    public Vector3 tPrevPos = Vector3.zero; // The previous touch position
    public Vector3 tPosDelta = Vector3.zero; // The difference between the current touch position and the previous touch position

    private Touch touch; // Used to store information about the users touch
    private Vector3 tCurrentPos = Vector3.zero; // The current touch position

    private RaycastHit hit; // Used to store information about what the Raycast hits

    private bool isBeingSpun; // Bool to check if the user is spinning the object

    private Animator animator; // The shape objects animator for triggering animations

    public bool swipedDown = false; // Boolean to check if the user has swiped down on the shape
    private float timeElapsed = 0f; // Float to control the time that has elapsed since the lerp started
    private float lerpDuration = 5f; // Float to control how long it takes for the shape to animate up and down
    [SerializeField] private float lerpWaitTime = 0.0f; // float to control how long the shape should wait before lerping
    private bool lerpDown = false; // Boolean to check if the shape has animated down
    private bool lerpUp = false; // Boolean to check if the shape has animated up
    private bool isAnimating = false; // Boolean to check if the shape is animating
    [SerializeField] private GameObject textOnPlane; // GameObject to enable and disable the text on the plane

    // Start is called before the first frame update
    // Get all of the references that are needed
    void Start()
    {
        arCamera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
        textOnPlane.GetComponent<TextMeshProUGUI>().text = prefabHolder.name;
    }

    // Update is called once per frame
    void Update()
    {
        // If the shape isn't swiped down, meaning that it is visible
        if (!swipedDown)
        {
            // If the shape object should be animating up and isn't at the right position, then animate its position
            if (!Mathf.Approximately(transform.position.y, (prefabHolder.transform.position.y + multiplier)) && lerpUp)
            {
                if (timeElapsed < lerpDuration)
                {
                    transform.position = Vector3.Lerp(transform.position, (prefabHolder.transform.position + (prefabHolder.transform.up * multiplier)), timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);
                    timeElapsed = 0f;
                    lerpUp = false;
                }
            }
            else
            {
                transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);
            }

            // UNUSED
            //transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);

            isBeingSpun = false;

            // If the user is touching the screen but hasn't triple tapped
            if (Input.touches.Length > 0 && Input.touches.Length < 2)
            {
                // Get the touch and set the current touch position
                touch = Input.touches[0];
                tCurrentPos = new Vector3(touch.position.x, touch.position.y, 0f); 

                // Fire a Raycast from the users touch point
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // If the raycast his this object
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject)
                {
                    // If the user has swiped down on the object and its not already animating then run the function to hide the shape and reveal the text on the plane
                    if (touch.deltaPosition.y < -30f && !isAnimating)
                    {
                        StartCoroutine(SwipeDown());
                        return;
                    }

                    // If the user has double tapped, call the AnimateOut function
                    if (touch.tapCount >= 2)
                    {
                        StartCoroutine(AnimateOut());
                    }

                    // Calculate the difference between the current touch position and the previous touch position
                    tPosDelta = tCurrentPos - tPrevPos;

                    // Rotate the object based on how much the user has swiped
                    transform.RotateAround(transform.position, Vector3.up, -(tPosDelta.x * sensitivity));

                    // UNUSED
                    //transform.Rotate(transform.up, (-Vector3.Dot(tPosDelta, Vector3.right) * sensitivity), Space.Self);
                    //transform.Rotate(0f, (tPosDelta.x * sensitivity), 0f, Space.World);

                    // Set the boolean to true
                    isBeingSpun = true;
                }
            }
            // If the user has triple tapped
            else if (Input.touches.Length > 0 && Input.touches.Length < 3)
            {

            }

            // If the shape object is not being spun by the player, rotate the object at a fixed speed
            if (!isBeingSpun)
            {
                // Debug.Log for testing
                Debug.Log("isBeingSpun is false");
                transform.RotateAround(transform.position, Vector3.up, -(spinningSpeed / 10f));

                // UNUSED
                //transform.Rotate(transform.up, Vector3.Dot(new Vector3(-(spinningSpeed / 10f), 0f, 0f), Vector3.right), Space.Self);
                //transform.Rotate(0f, 1 * sensitivity, 0f, Space.World);
            }

            // Set the previous touch position to the current touch position
            tPrevPos = tCurrentPos;
        }
        // If the shape object is swiped down
        else
        {
            // If the shape object isn't where its supposed to be, animate it into the correct position
            if (!Mathf.Approximately(transform.position.y, prefabHolder.transform.position.y) && lerpDown)
            {
                if (timeElapsed < lerpDuration)
                {
                    transform.position = Vector3.Lerp(transform.position, prefabHolder.transform.position, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    timeElapsed = 0f;
                    transform.position = prefabHolder.transform.position;
                    lerpDown = false;
                }
            }
            // If the user is touching the screen
            if (Input.touches.Length > 0)
            {
                // Get the users touch
                touch = Input.touches[0];

                // Fire a Raycast from the users touch point
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // If the raycast hits the plane object and the user has swiped up on the plane object, animate the shape to its normal position
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == planeObj)
                {
                    if (touch.deltaPosition.y > 30f && !isAnimating)
                    {
                        StartCoroutine(SwipeUp());
                    }
                }
            }
        }
    }

    // FixedUpdate is called 50 times per second
    void FixedUpdate()
    {

    }

    // Coroutine that is called when the object is swiped down
    public IEnumerator SwipeDown()
    {
        // If the object isn't already animating
        if (!isAnimating)
        {
            // Debug.Log for testing
            Debug.Log(this.gameObject.name + " Swipe Down");

            // UNUSED
            //animator.SetTrigger("Exit");

            // Play the exit animation
            animator.Play("Base Layer.ExitAnimation");

            // Set the boolean to true
            lerpDown = true;

            // Wait for the amount of time
            yield return new WaitForSeconds(lerpWaitTime);

            // Set the text on the plan to active
            textOnPlane.SetActive(true);

            // UNUSED
            //transform.position = prefabHolder.transform.position;
            //transform.localScale -= transform.localScale * 0.1f;

            // Set the booleans to true
            swipedDown = true;
            isAnimating = true;

            // Start the coroutine to change the isAnimating bool, this prevents accidentally swiping up immediately after swiping down
            StartCoroutine(IsAnimatingChanger());
        }
    }

    // Coroutine that is called when the object is swiped up
    public IEnumerator SwipeUp()
    {
        // If the object isn't already animating
        if (!isAnimating)
        {
            // Debug.Log for testing
            Debug.Log(this.gameObject.name + " Swipe Up");

            // UNUSED
            //animator.SetTrigger("Enter");


            // Play the exit animation
            animator.Play("Base Layer.EnterAnimation");

            // Set the boolean to true
            lerpUp = true;

            // Set the boolean to false
            swipedDown = false;


            // Wait for the amount of time
            yield return new WaitForSeconds(0f);

            // UNUSED
            //transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);
            //transform.localScale += transform.localScale * 10f;

            // Set the text on the plane to inactive
            textOnPlane.SetActive(false);

            // Set the boolean to true
            isAnimating = true;

            // Start the coroutine to change the isAnimating bool, this prevents accidentally swiping down immediately after swiping up
            StartCoroutine(IsAnimatingChanger());
        }
    }

    // Coroutine that changes the isAnimating bool after a delay
    IEnumerator IsAnimatingChanger()
    {
        yield return new WaitForSeconds(1f);
        isAnimating = false;
    }

    // Coroutine that plays the exit animation and enables the Infocard object after a delay
    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.Play("Base Layer.ExitAnimation");

        // UNUSED
        //animator.SetTrigger("Exit");

        yield return new WaitForSeconds(1f);

        infoCard.SetActive(true);

        // UNUSED
        //infoCard.GetComponent<Infocard>().AnimateIn();

        infoCard.SendMessage("AnimateIn");

        // Disable this GameObject
        this.gameObject.SetActive(false);
    }

    // Called by the Infocard object when it animates out
    public void AnimateIn()
    {
        // Play the enter animation
        animator.Play("Base Layer.EnterAnimation");

        // UNUSED
        //animator.SetTrigger("Enter");
    }

    // UNUSED
    //void OnBecameInvisible()
    //{
        //this.gameObject.SetActive(false);
    //}
}