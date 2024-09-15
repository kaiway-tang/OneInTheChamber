using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFragment : MonoBehaviour
{
    [SerializeField] float launchPower;
    [SerializeField] Rigidbody rb;
    Vector3 spin;
    float spinMag = 5;
    void Start()
    {
        Vector3 launchDir = Random.insideUnitSphere.normalized;
        if (launchDir.y < 0) { launchDir.y *= -1; }
        rb.velocity = launchDir * launchPower * Random.Range(0.6f, 1.4f);
        Destroy(gameObject, 5);
        
        spin.x = Random.Range(-spinMag, spinMag);
        spin.y = Random.Range(-spinMag, spinMag);
        spin.z = Random.Range(-spinMag, spinMag);
    }

    void FixedUpdate()
    {
        transform.Rotate(spin);
    }
}
