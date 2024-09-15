using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCrystal : MonoBehaviour
{
    [SerializeField] Transform modelTrfm;
    [SerializeField] GameObject crystalFrag;
    [SerializeField] GameObject shatterSFX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        modelTrfm.Rotate(Vector3.up * 3f);
    }

    int startGame;
    public void End()
    {
        for (int i = 0; i < Random.Range(5, 8); i++)
        {
            Instantiate(crystalFrag, transform.position, Quaternion.identity);
        }
        HTN_Tutorial.StartGame();
        gameObject.SetActive(false);
        Instantiate(shatterSFX, transform.position, Quaternion.identity);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            End();
        }
    }
}
