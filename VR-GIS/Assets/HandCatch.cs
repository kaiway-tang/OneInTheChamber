using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCatch : MonoBehaviour
{
    public Arrow caughtArrow;
    [SerializeField] Collider catchCol;
    [SerializeField] GameObject catchDebug;
    int catchTimer;
    public bool arrowInHand, handClosed, onHandClosed;
    OVRInput.Controller controller;
    [SerializeField] bool right;
    [SerializeField] AudioSource catchSFX;
    // Start is called before the first frame update
    void Start()
    {
        if (right)
        {
            controller = OVRInput.Controller.RTouch;
        }
        else
        {
            controller = OVRInput.Controller.LTouch;
        }
    }

    private void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handClosed = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) > 0.5f;
        if (onHandClosed != handClosed)
        {
            onHandClosed = handClosed;
            if (onHandClosed)
            {
                Catch();
            }
        }

        if (catchTimer > 0)
        {
            catchTimer--;
            if (catchTimer == 0)
            {
                catchCol.enabled = false;
                catchDebug.SetActive(false);
            }
        }

        if (arrowInHand)
        {
            if (!handClosed)
            {
                caughtArrow.End();
            }
        }
    }

    void Catch()
    {
        catchDebug.SetActive(true);
        catchCol.enabled = true;
        catchTimer = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            catchSFX.Play();
            Arrow otherArrow = other.GetComponent<Arrow>();

            float dotP = Vector3.Dot(otherArrow.trfm.forward, transform.forward);
            if (!otherArrow.flying || (dotP < 0.7f && dotP > -0.7f)) { return; }

            caughtArrow = otherArrow;

            caughtArrow.Catch();
            caughtArrow.trfm.parent = transform;
            caughtArrow.trfm.rotation = transform.rotation;
            caughtArrow.trfm.position = transform.position;

            arrowInHand = true;
        }
    }
}
