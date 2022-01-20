using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator), typeof(LookAtConstraint))]
public class Infocard : MonoBehaviour
{
    [SerializeField] private GameObject prefabHolder;
    [SerializeField] private GameObject objectObj;

    [SerializeField] private float multiplier;

    private Animator animator;

    private LookAtConstraint lookAtConstraint;
    private ConstraintSource cameraSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        lookAtConstraint = GetComponent<LookAtConstraint>();
    }

    // Update is called once per frame
    void Update()
    {
        // NOTE: NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
        multiplier = (((transform.localRotation.eulerAngles.x - 0) * 0.15f) / 90) + 0.05f;

        transform.position = prefabHolder.transform.position + (Vector3.up * multiplier);
    }

    IEnumerator AnimateOut()
    {
        // Play the exit animation
        animator.Play("Base Layer.ExitAnimation");

        yield return new WaitForSeconds(1f);

        objectObj.SetActive(true);

        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        cameraSource.sourceTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        lookAtConstraint.AddSource(cameraSource);
        lookAtConstraint.constraintActive = true;
        // Play the enter animation
        animator.Play("Base Layer.EnterAnimation");
    }
}
