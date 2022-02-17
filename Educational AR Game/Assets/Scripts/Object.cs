using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Object : MonoBehaviour
{
    [SerializeField] private GameObject prefabHolder;
    [SerializeField] private GameObject infoCard;
    [SerializeField] private GameObject arCamera;
    [SerializeField] private GameObject planeObj;

    public float multiplier;

    [SerializeField] private float sensitivity = 0.5f;

    [SerializeField] private float spinningSpeed = 7f;

    public Vector3 tPrevPos = Vector3.zero;
    public Vector3 tPosDelta = Vector3.zero;

    private Touch touch;
    private Vector3 tCurrentPos = Vector3.zero;

    private RaycastHit hit;

    private bool isBeingSpun;

    private Animator animator;

    public bool swipedDown = false;
    private float timeElapsed = 0f;
    private float lerpDuration = 5f;
    [SerializeField] private float lerpWaitTime = 0.0f;
    private bool lerpDown = false;
    private bool lerpUp = false;
    private bool isAnimating = false;
    [SerializeField] private GameObject textOnPlane;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
        textOnPlane.GetComponent<TextMeshProUGUI>().text = prefabHolder.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (!swipedDown)
        {
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

            //transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);

            isBeingSpun = false;

            if (Input.touches.Length > 0 && Input.touches.Length < 2)
            {
                touch = Input.touches[0];
                tCurrentPos = new Vector3(touch.position.x, touch.position.y, 0f); 

                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject)
                {
                    if (touch.deltaPosition.y < -30f && !isAnimating)
                    {
                        StartCoroutine(SwipeDown());
                        return;
                    }

                    if (touch.tapCount >= 2)
                    {
                        StartCoroutine(AnimateOut());
                    }
                    tPosDelta = tCurrentPos - tPrevPos;
                    transform.RotateAround(transform.position, Vector3.up, -(tPosDelta.x * sensitivity));
                    //transform.Rotate(transform.up, (-Vector3.Dot(tPosDelta, Vector3.right) * sensitivity), Space.Self);
                    //transform.Rotate(0f, (tPosDelta.x * sensitivity), 0f, Space.World);
                    isBeingSpun = true;
                }
            }
            else if (Input.touches.Length > 0 && Input.touches.Length < 3)
            {

            }

            if (!isBeingSpun)
            {
                Debug.Log("isBeingSpun is false");
                transform.RotateAround(transform.position, Vector3.up, -(spinningSpeed / 10f));
                //transform.Rotate(transform.up, Vector3.Dot(new Vector3(-(spinningSpeed / 10f), 0f, 0f), Vector3.right), Space.Self);
                //transform.Rotate(0f, 1 * sensitivity, 0f, Space.World);
            }

            tPrevPos = tCurrentPos;
        }
        else
        {
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
            if (Input.touches.Length > 0)
            {
                touch = Input.touches[0];

                Ray ray = Camera.main.ScreenPointToRay(touch.position);

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

    void FixedUpdate()
    {

    }

    public IEnumerator SwipeDown()
    {
        if (!isAnimating)
        {
            Debug.Log(this.gameObject.name + " Swipe Down");
            //animator.SetTrigger("Exit");
            animator.Play("Base Layer.ExitAnimation");
            lerpDown = true;
            yield return new WaitForSeconds(lerpWaitTime);
            textOnPlane.SetActive(true);
            //transform.position = prefabHolder.transform.position;
            //transform.localScale -= transform.localScale * 0.1f;
            swipedDown = true;
            isAnimating = true;
            StartCoroutine(IsAnimatingChanger());
        }
    }

    public IEnumerator SwipeUp()
    {
        if (!isAnimating)
        {
            Debug.Log(this.gameObject.name + " Swipe Up");
            //animator.SetTrigger("Enter");
            animator.Play("Base Layer.EnterAnimation");
            lerpUp = true;
            swipedDown = false;
            yield return new WaitForSeconds(0f);
            //transform.position = prefabHolder.transform.position + (prefabHolder.transform.up * multiplier);
            //transform.localScale += transform.localScale * 10f;
            textOnPlane.SetActive(false);
            isAnimating = true;
            StartCoroutine(IsAnimatingChanger());
        }
    }

    IEnumerator IsAnimatingChanger()
    {
        yield return new WaitForSeconds(1f);
        isAnimating = false;
    }

    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.Play("Base Layer.ExitAnimation");
        //animator.SetTrigger("Exit");

        yield return new WaitForSeconds(1f);

        infoCard.SetActive(true);
        //infoCard.GetComponent<Infocard>().AnimateIn();
        infoCard.SendMessage("AnimateIn");

        this.gameObject.SetActive(false);
    }

    public void AnimateIn()
    {
        // Play the enter animation
        animator.Play("Base Layer.EnterAnimation");
        //animator.SetTrigger("Enter");
    }

    //void OnBecameInvisible()
    //{
        //this.gameObject.SetActive(false);
    //}
}