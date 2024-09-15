using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public bool alive;
    public Crystal nearestNeigh;
    bool hasNearestNeigh;
    public Transform trfm;
    public int id;
    [SerializeField] float speed;
    [SerializeField] Transform modelTrfm;
    [SerializeField] GameObject crystalFrag;
    [SerializeField] GameObject shatterSFX;
    [SerializeField] AudioSource electricSFX, explosionSFX;

    bool ended;
    // Start is called before the first frame update
    void Start()
    {
        hasNearestNeigh = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { End(); }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!hasNearestNeigh || !nearestNeigh.alive) { GetNearestNeighbor(); }

        if (hasNearestNeigh && !ended)
        {            
            trfm.position += (nearestNeigh.trfm.position - trfm.position).normalized * speed;
            if (Vector3.SqrMagnitude(trfm.position - nearestNeigh.trfm.position) < .16f)
            {
                //electricSFX.Play();
                if (Vector3.SqrMagnitude(trfm.position - nearestNeigh.trfm.position) < .04f)
                {
                    //explosionSFX.Play();
                    HTNManager.self.GameOver();
                    ended = true;
                }
            }
            else if (electricSFX.isPlaying)
            {
                electricSFX.Stop();
            }
        }

        modelTrfm.Rotate(Vector3.up * 3f);
    }

    void GetNearestNeighbor()
    {
        float minDist = 99999;
        float sqrDist = 0;
        foreach (Crystal crystal in HTNManager.self.crystals)
        {
            if (crystal.alive && crystal.id != id)
            {
                sqrDist = Vector3.SqrMagnitude(trfm.position - crystal.trfm.position);
                if (sqrDist < minDist)
                {
                    minDist = sqrDist;
                    nearestNeigh = crystal;
                }         
            }
        }

        hasNearestNeigh = minDist < 99999;
    }

    public void End()
    {
        HTNManager.self.levelActiveCrystals--;
        alive = false;
        for (int i = 0; i < Random.Range(5,8); i++)
        {
            Instantiate(crystalFrag, trfm.position, Quaternion.identity);
        }
        Instantiate(shatterSFX, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            End();
        }
    }
}
