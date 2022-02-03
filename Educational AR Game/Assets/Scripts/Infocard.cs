using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Animator), typeof(LookAtConstraint))]
public class Infocard : MonoBehaviour
{
    [SerializeField] private GameObject prefabHolder;
    [SerializeField] private GameObject objectObj;

    [SerializeField] private GameObject cameraObj;

    [SerializeField] private float multiplier;

    private Animator animator;

    private LookAtConstraint lookAtConstraint;
    private ConstraintSource cameraSource;

    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //lookAtConstraint = GetComponent<LookAtConstraint>();
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // NOTE: NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
        multiplier = (((transform.localRotation.eulerAngles.x - 0) * 0.15f) / 90) + 0.05f;

        transform.position = prefabHolder.transform.position + (Vector3.up * multiplier);

        if (isActive)
        {
            cameraObj = GameObject.Find("Camera Holder");
            isActive = false;
        }

        //transform.LookAt(cameraObj.transform);
        transform.LookAt(Camera.main.transform);
    }

    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.Play("Base Layer.ExitAnimation");

        yield return new WaitForSeconds(1f);

        objectObj.GetComponent<Object>().AnimateIn();
        objectObj.SetActive(true);

        this.gameObject.SetActive(false);
    }

    public void AnimateIn()
    {
        //cameraSource.sourceTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //lookAtConstraint.AddSource(cameraSource);
        //lookAtConstraint.constraintActive = true;

        // Play the enter animation
        animator.Play("Base Layer.EnterAnimation");
    }
}
