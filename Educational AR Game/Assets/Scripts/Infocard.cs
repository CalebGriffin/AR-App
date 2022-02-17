using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Infocard : MonoBehaviour
{
    [SerializeField] protected GameObject prefabHolder;
    [SerializeField] protected GameObject objectObj;

    [SerializeField] protected GameObject cameraObj;
    [SerializeField] protected GameObject textHolder;

    [SerializeField] protected float multiplier;

    protected Animator animator;

    //public bool isActive = false;

    //[SerializeField] protected TextMeshProUGUI infocardText;

    protected Touch touch;

    protected RaycastHit hit;

    protected int currentTextbox = 0;

    [SerializeField] protected TextMeshProUGUI[] textboxObjs;
    [SerializeField] protected GameObject rightButton;
    [SerializeField] protected GameObject leftButton;
    [SerializeField] protected GameObject canvasObj;
    protected Canvas canvas;
    protected float timeElapsed;
    [SerializeField] protected float lerpDuration = 0.5f;

    //protected string[] shapeNames2D = new string[] {"Circle", "Triangle", "Square", "Pentagon", "Hexagon", "Heptagon", "Octagon"};
    //protected string[] shapeNames3D = new string[] {"Cylinder", "Triangular Prism", "Cube", "Pentagon Based Pyramid", "Hexagonal Prism", "Square Based Pyramid", "Sphere"};
    //[SerializeField] private ShapeInfo2D shapeInfo2D;
    //[SerializeField] private ShapeInfo3D shapeInfo3D;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        cameraObj = GameObject.Find("Camera Holder");
        canvas = canvasObj.GetComponent<Canvas>();
        canvas.worldCamera = cameraObj.GetComponent<CameraHolder>().GetCamera();
        multiplier = objectObj.GetComponent<Object>().multiplier;
        TextSetUp();
    }

    protected virtual void TextSetUp()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // NOTE: NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
        //multiplier = (((-transform.localRotation.eulerAngles.x - 0) * 0.15f) / 90) + 0.05f;

        //transform.position = prefabHolder.transform.position + (Vector3.up * -(multiplier / 2));
        transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);


        transform.LookAt(cameraObj.transform, cameraObj.transform.up);
        transform.Rotate(-90f, -180f, 0f);
        //canvasObj.transform.LookAt(cameraObj.transform, cameraObj.transform.forward);
        //canvasObj.transform.Rotate(180f, 0f, 180f);

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

        //infocardText.text = $@"X: {transform.localRotation.x}
        //Y: {transform.localRotation.y}
        //Z: {transform.localRotation.z}
        //Multiplier: {multiplier}";
    }

    void FixedUpdate()
    {
        Debug.Log("Fixed Update");
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

    public void LeftButton()
    {
        Debug.Log("Left Button Pressed");
        currentTextbox -= 1;
    }

    public void RightButton()
    {
        Debug.Log("Right Button Pressed");
        currentTextbox += 1;
    }

    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.SetTrigger("Exit");

        yield return new WaitForSeconds(1f);

        objectObj.SetActive(true);
        objectObj.GetComponent<Object>().AnimateIn();

        this.gameObject.SetActive(false);
    }

    public void AnimateIn()
    {
        // Play the enter animation
        animator.SetTrigger("Enter");
    }
}