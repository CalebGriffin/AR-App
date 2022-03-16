using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using TMPro;

// If no component is found then one will be added automatically, helping to prevent reference errors
[RequireComponent(typeof(Animator))]
public abstract class Infocard : MonoBehaviour
{
    [SerializeField] protected GameObject prefabHolder; // Reference to the parent object of the shape
    [SerializeField] protected GameObject objectObj; // Reference to the shape object so that it can trigger animations

    [SerializeField] protected GameObject cameraObj; // Reference to the camera object so that it can look at the camera
    [SerializeField] protected GameObject textHolder; // The parent of the text on the infocard so that it can control which text is being shown

    [SerializeField] protected float multiplier; // A float controlling how high the infocard hovers above the tracked image

    protected Animator animator; // A reference to the objects Animator so that it can trigger animations

    // UNUSED
    //public bool isActive = false;
    //[SerializeField] protected TextMeshProUGUI infocardText;

    protected Touch touch; // Used to detect when the user has touched the screen

    protected RaycastHit hit; // Used for the output of the raycast when the user has touched the screen

    protected int currentTextbox = 0; // An int to keep track of which text box is currently being displayed on the infocard

    [SerializeField] protected TextMeshProUGUI[] textboxObjs; // An array of text objects used to know the number of text object on the infocard
    [SerializeField] protected GameObject rightButton; // Used to be able to show and hide the right button
    [SerializeField] protected GameObject leftButton; // Used to be able to show and hide the left button
    [SerializeField] protected GameObject canvasObj; // UNUSED
    protected Canvas canvas; // UNUSED
    protected float timeElapsed; // Float used to keep track of how much time has elapsed between each lerp
    [SerializeField] protected float lerpDuration = 0.5f; // Float used to control how long the text should take to transition to the next text box

    // UNUSED
    //protected string[] shapeNames2D = new string[] {"Circle", "Triangle", "Square", "Pentagon", "Hexagon", "Heptagon", "Octagon"};
    //protected string[] shapeNames3D = new string[] {"Cylinder", "Triangular Prism", "Cube", "Pentagon Based Pyramid", "Hexagonal Prism", "Square Based Pyramid", "Sphere"};
    //[SerializeField] private ShapeInfo2D shapeInfo2D;
    //[SerializeField] private ShapeInfo3D shapeInfo3D;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Awake is called on the first frame update
    // Get references to all of the components that are needed and run the TextSetUp function
    void Awake()
    {
        animator = GetComponent<Animator>();
        cameraObj = GameObject.Find("Camera Holder");
        canvas = canvasObj.GetComponent<Canvas>();
        canvas.worldCamera = cameraObj.GetComponent<CameraHolder>().GetCamera();
        multiplier = objectObj.GetComponent<Object>().multiplier;
        TextSetUp();
    }

    // This is an abstract function which will be overridden by the inherited classes to set up the text for each of the text boxes
    protected abstract void TextSetUp();

    // Update is called once per frame
    void Update()
    {
        // UNUSED
        // NOTE: NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
        //multiplier = (((-transform.localRotation.eulerAngles.x - 0) * 0.15f) / 90) + 0.05f;
        //transform.position = prefabHolder.transform.position + (Vector3.up * -(multiplier / 2));

        // Set the position of the infocard to above the position of the parent object
        transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);


        // Look at the camera object
        transform.LookAt(cameraObj.transform, cameraObj.transform.up);
        transform.Rotate(-90f, -180f, 0f);

        // UNUSED
        //canvasObj.transform.LookAt(cameraObj.transform, cameraObj.transform.forward);
        //canvasObj.transform.Rotate(180f, 0f, 180f);

        // If the user is touching the screen, fire a Raycast from their touch, if they double tap on the Infocard then animate out and spawn the shape object
        if (Input.touches.Length > 0)
        {
            touch = Input.touches[0];

            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject)
            {
                if (touch.tapCount >= 2)
                {
                    StartCoroutine(AnimateOut());
                }
            }
        }

        // UNUSED
        //infocardText.text = $@"X: {transform.localRotation.x}
        //Y: {transform.localRotation.y}
        //Z: {transform.localRotation.z}
        //Multiplier: {multiplier}";
    }

    // Fixed update is called 50 times per second
    void FixedUpdate()
    {
        // Debug.Log for testing
        Debug.Log("Fixed Update");

        // Set the left or right buttons to inactive if there is no text box to scroll to, else set them both to active
        if (currentTextbox == 0)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(true);
        }
        else if (currentTextbox == textboxObjs.Length - 1)
        {
            leftButton.SetActive(true);
            rightButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
        }

        // If the current position of the text object is not set to the current text box then lerp to that position over time, else snap to that position and reset the lerp
        if (!Mathf.Approximately(textHolder.transform.localPosition.x, -currentTextbox * 1200f))
        {
            if (timeElapsed < lerpDuration)
            {
                textHolder.transform.localPosition = Vector3.Lerp(textHolder.transform.localPosition, new Vector3(-currentTextbox * 1200f, textHolder.transform.localPosition.y, textHolder.transform.localPosition.z), timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                timeElapsed = 0;
                textHolder.transform.localPosition = new Vector3(-currentTextbox * 1200f, textHolder.transform.localPosition.y, textHolder.transform.localPosition.z);
            }
        }
    }

    // Called when the left button is pressed, sets the current text box back by 1
    public void LeftButton()
    {
        // Debug.Log for testing
        Debug.Log("Left Button Pressed");
        currentTextbox -= 1;
    }

    // Called when the right button is pressed, sets the current text box forward by 1
    public void RightButton()
    {
        // Debug.Log for testing
        Debug.Log("Right Button Pressed");
        currentTextbox += 1;
    }

    // A coroutine that will animate the infocard shrinking and then spawn the shape object and tell it to animate in
    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.SetTrigger("Exit");

        yield return new WaitForSeconds(1f);

        objectObj.SetActive(true);
        objectObj.GetComponent<Object>().AnimateIn();

        this.gameObject.SetActive(false);
    }

    // Called by the shape object, animates the entrance of the infocard
    public void AnimateIn()
    {
        // Play the enter animation
        animator.SetTrigger("Enter");
    }
}