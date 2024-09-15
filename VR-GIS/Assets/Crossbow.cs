using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] Transform loadPos, drawPos;
    [SerializeField] HandCollider drawCol;

    [SerializeField] Arrow arrow;
    public int status;
    public const int REST = 0, LOADED = 1, DRAWING = 2, DRAWN = 3;

    OVRInput.Controller controller;
    [SerializeField] bool right;
    [SerializeField] Transform otherHand;
    [SerializeField] GameObject laserObj;
    float drawDist;

    [SerializeField] Transform[] bends; //left, right, lsub, rsub
    [SerializeField] AudioSource fireSFX, drawSFX;
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

        drawDist = Vector3.Distance(loadPos.position, drawPos.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (status == DRAWN)
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > 0.5f)
            {
                Fire();
            }
        }

        if (status == DRAWING)
        {
            float dist = Vector3.Distance(otherHand.position, loadPos.position)/drawDist;
            arrow.trfm.position = loadPos.position * (1 - dist) + drawPos.position * dist;

            if (dist > 0.9f) { TransformArm(0.9f); }
            else
            {
                TransformArm(dist);
            }
        }

        //TransformArm(taa);
    }
    [SerializeField] float taa;

    void Fire()
    {
        arrow.SetFlying(true);
        arrow.transform.parent = null;
        arrow.life = 150;
        arrow.speed = .4f; //0.3f
        arrow.trfm.localScale *= 1/0.6f;
        status = REST;
        laserObj.SetActive(false);
        TransformArm(0);
        fireSFX.Play();
    }

    public bool LoadArrow(Arrow pArrow)
    {
        if (status != REST) { return false; }

        arrow = pArrow;
        pArrow.trfm.parent = transform;
        pArrow.trfm.rotation = transform.rotation;
        status = DRAWING;
        drawSFX.Play();
        return true;
    }

    public void Drawn()
    {
        arrow.trfm.position = drawPos.position;
        status = DRAWN;
        laserObj.SetActive(true);
        TransformArm(.9f);
    }

    Vector3 tempVect;
    public void TransformArm(float percent) // 0 - 1.0
    {
        percent = Mathf.Acos(percent) * 180 / 3.1415f;
        tempVect.y = percent;
        bends[0].localEulerAngles = tempVect;

        tempVect.y = -percent;
        bends[1].localEulerAngles = tempVect;

        tempVect.y = 200 - percent * 2.22f;
        bends[2].localEulerAngles = tempVect;

        tempVect.y = -200 + percent * 2.22f;
        bends[3].localEulerAngles = tempVect;
    }
}
