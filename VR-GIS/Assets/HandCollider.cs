using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    public int use;
    public const int LOAD = 1, DRAW = 2, HAND = 0;
    [SerializeField] bool right;
    public Crossbow crossbow;
    [SerializeField] HandCatch handCatch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int touchCount;
    public bool Touching()
    {
        return touchCount > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            touchCount += 1;
            if (use == HAND)
            {
                HandCollider HCol = other.GetComponent<HandCollider>();
                if (HCol.use == LOAD && handCatch.arrowInHand)
                {
                    if (HCol.crossbow.LoadArrow(handCatch.caughtArrow))
                    {
                        handCatch.arrowInHand = false;
                    }                    
                }

                if (HCol.use == DRAW && HCol.crossbow.status == Crossbow.DRAWING)
                {
                    HCol.crossbow.Drawn();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            touchCount -= 1;
        }
    }
}
