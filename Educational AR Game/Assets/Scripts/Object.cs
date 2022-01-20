using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Object : MonoBehaviour
{
    [SerializeField] private GameObject prefabHolder;
    [SerializeField] private GameObject infoCard;
    [SerializeField] private GameObject arCamera;

    [SerializeField] private float multiplier;

    [SerializeField] private float sensitivity = 0.5f;

    [SerializeField] private float spinningSpeed = 7f;

    public Vector3 tPrevPos = Vector3.zero;
    public Vector3 tPosDelta = Vector3.zero;

    private Touch touch;
    private Vector3 tCurrentPos = Vector3.zero;

    private RaycastHit hit;

    private bool isBeingSpun;

    private float touchTime;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = prefabHolder.transform.position + (Vector3.up * multiplier);

        isBeingSpun = false;

        if (Input.touches.Length > 0)
        {
            touch = Input.touches[0];
            tCurrentPos = new Vector3(touch.position.x, touch.position.y, 0f); 

            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == this.gameObject)
            {
                touchTime += Time.deltaTime;
                tPosDelta = tCurrentPos - tPrevPos;
                transform.Rotate(transform.up, (-Vector3.Dot(tPosDelta, arCamera.transform.right) * sensitivity), Space.World);
                isBeingSpun = true;
            }
            else
            {
                touchTime = 0f;
            }
        }

        if (touchTime >= 1f)
        {
            StartCoroutine(AnimateOut());
        }

        if (!isBeingSpun)
        {
            Debug.Log("isBeingSpun is false");
            transform.Rotate(transform.up, Vector3.Dot(new Vector3(-(spinningSpeed / 100f), 0f, 0f), arCamera.transform.right), Space.World);
        }

        tPrevPos = tCurrentPos;
    }

    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.Play("Base Layer.ExitAnimation");

        yield return new WaitForSeconds(1f);

        infoCard.SetActive(true);

        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        // Play the enter animation
        animator.Play("Base Layer.EnterAnimation");
    }

    //void OnBecameInvisible()
    //{
        //this.gameObject.SetActive(false);
    //}
}
