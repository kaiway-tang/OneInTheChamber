using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Transform head;
    public static Player self;
    [SerializeField] SpriteRenderer dmgVignette;
    Color dmgVigCol;
    float dmgVigTmr;
    [SerializeField] AudioSource deathSFX;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Player>();
        dmgVigCol = dmgVignette.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (dmgVigTmr > 0)
        {
            dmgVigTmr -= Time.deltaTime;
            if (dmgVigTmr <= 0) { dmgVigTmr = 0; }
            dmgVigCol.a = dmgVigTmr / 1;
            dmgVignette.color = dmgVigCol;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Arrow otherArrow = other.GetComponent<Arrow>();
            if (otherArrow.flying && !otherArrow.friendly)
            {
                dmgVigTmr = 1;
                otherArrow.End();
                deathSFX.Play();
                HTNManager.self.Respawn();
            }
        }
    }
}
