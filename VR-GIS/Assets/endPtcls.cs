using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endPtcls : MonoBehaviour
{
    [SerializeField] ParticleSystem ptclSys;
    public void Activate()
    {
        transform.parent = null;
        ptclSys.Play();
        Destroy(gameObject, ptclSys.duration + ptclSys.startLifetime);
    }
}
