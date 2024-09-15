using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    public int cooldown;
    Vector3 restPos, activePos;
    bool active;
    [SerializeField] AudioSource fireSFX, slideSFX;
    // Start is called before the first frame update
    void Start()
    {
        restPos = transform.position + Vector3.up * -2;
        activePos = transform.position;
        transform.position = restPos;
        active = false;
        cooldown = 1;
    }

    int initiateTimer, resetTimer, fireRate;
    public void Initiate(int baseFireRate)
    {
        initiateTimer = 50;

        fireRate = baseFireRate;
        cooldown = Mathf.RoundToInt(Random.Range(0.2f,.6f) * 200);
        active = true;
        slideSFX.Play();
    }

    public void ResetPos()
    {
        resetTimer = 50;
        active = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.forward = Player.self.head.position - transform.position;
        if (cooldown > 0)
        {
            if (active) { cooldown--; }
        }
        else
        {
            cooldown = Mathf.RoundToInt(Random.Range(0.8f,1.2f) * fireRate);
            Instantiate(arrow, transform.position, transform.rotation);
            fireSFX.Play();
        }

        if (initiateTimer > 0)
        {
            initiateTimer--;
            transform.position += (activePos - transform.position) * 0.1f;
        }

        if (resetTimer > 0)
        {
            resetTimer--;
            transform.position += (restPos - transform.position) * 0.1f;
        }
    }
}
