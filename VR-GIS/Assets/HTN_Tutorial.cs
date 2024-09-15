using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HTN_Tutorial : MonoBehaviour
{
    [SerializeField] Dispenser dispenser;
    [SerializeField] GameObject arrowPrefab, arrowObj;
    [SerializeField] Transform targetPos;
    int timer;
    static HTN_Tutorial self;
    [SerializeField] AudioSource dispenserAppear, dispenserFire;
    // Start is called before the first frame update
    void Start()
    {
        FadeOutBlack();
        self = GetComponent<HTN_Tutorial>();
    }

    public static void StartGame()
    {
        self.startGameTimer = 100;
    }

    int startGameTimer;
    void FixedUpdate()
    {
        if (startGameTimer > 0)
        {
            startGameTimer--;
            if (startGameTimer == 50) { FadeToBlack(); }
            if (startGameTimer == 0)
            {
                SceneManager.LoadScene("HTN");
            }
        }

        HandleFade();
        timer++;

        if (timer > 100)
        {
            dispenser.Initiate(9999);
            dispenser.cooldown = 9999;
            dispenserAppear.Play();
        }
        if (timer > 150 && startGameTimer < 1)
        {
            if (!arrowObj)
            {
                dispenserFire.Play();
                InstantiateTutorialArrow();
            }
            if (arrowObj.GetComponent<Arrow>().flying && !arrowObj.GetComponent<Arrow>().caught)
            {
                arrowObj.GetComponent<Arrow>().life = 99999;
                arrowObj.transform.position += (targetPos.position - arrowObj.transform.position) * 0.05f;
            }
        }
    }

    void InstantiateTutorialArrow()
    {
        arrowObj = Instantiate(arrowPrefab, dispenser.transform.position, Quaternion.identity);
        arrowObj.transform.forward = targetPos.position - arrowObj.transform.position;
        arrowObj.GetComponent<Arrow>().friendly = true;
    }

    [SerializeField] SpriteRenderer blackFade;
    int blackFadeTimer;
    Color col;

    public void FadeToBlack()
    {
        blackFadeTimer = -50;
    }

    public void FadeOutBlack()
    {
        blackFadeTimer = 50;
    }

    void HandleFade()
    {
        if (blackFadeTimer > 0)
        {
            blackFadeTimer--;
            col = blackFade.color;
            col.a = blackFadeTimer / 50f;
            blackFade.color = col;
        }

        if (blackFadeTimer < 0)
        {
            blackFadeTimer++;
            col = blackFade.color;
            col.a = 1 - (Mathf.Abs(blackFadeTimer) / 50f);
            blackFade.color = col;
        }
    }
}
