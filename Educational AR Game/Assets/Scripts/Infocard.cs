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
    [SerializeField] private GameObject prefabHolder;
    [SerializeField] private GameObject objectObj;

    [SerializeField] private GameObject cameraObj;

    [SerializeField] private float multiplier;

    private Animator animator;

    public bool isActive = false;

    [SerializeField] private TextMeshProUGUI infocardText;

    private Touch touch;

    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        cameraObj = GameObject.Find("Camera Holder");
        multiplier = objectObj.GetComponent<Object>().multiplier;
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

    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.Play("Base Layer.ExitAnimation");

        yield return new WaitForSeconds(1f);

        objectObj.transform.localScale = Vector3.zero;
        objectObj.SetActive(true);
        objectObj.GetComponent<Object>().AnimateIn();

        this.gameObject.SetActive(false);
    }

    public void AnimateIn()
    {
        // Play the enter animation
        animator.Play("Base Layer.EnterAnimation");
    }
}
