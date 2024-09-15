using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    [SerializeField] int fireDelay;
    [SerializeField] GameObject laserObj;
    public bool flying = false, friendly = false, caught;
    public Transform trfm;
    public int life;
    public TrailRenderer trailRend;
    [SerializeField] endPtcls disintegratePtcls;
    // Start is called before the first frame update
    void Start()
    {
        life = 150;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HTNManager.respawnTimer > 0)
        {
            End();
        }

        if (fireDelay > 0)
        {
            fireDelay--;
            if (fireDelay == 0)
            {
                laserObj.SetActive(false);
                SetFlying(true);
            }
        }

        if (flying)
        {
            transform.position += transform.forward * speed;
            life--;
            if (life < 1) { End(); }
        }
    }

    public void Catch()
    {
        if (!flying) { return; }
        caught = true;
        SetFlying(false);
        trfm.localScale = trfm.localScale * 0.6f;
        friendly = true;
    }

    public void SetFlying(bool pFlying)
    {
        flying = pFlying;
        trailRend.emitting = pFlying;
    }

    public void End()
    {
        disintegratePtcls.Activate();
        Destroy(gameObject);
    }
}
