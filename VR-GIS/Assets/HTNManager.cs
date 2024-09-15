using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HTNManager : MonoBehaviour
{
    public List<Crystal> crystals;
    public static HTNManager self;
    public int idRegister;
    [SerializeField] GameObject crystal;
    [SerializeField] float arenaSize;
    [SerializeField] Transform plate;
    [SerializeField] Dispenser[] dispensers;
    public List<Dispenser> activeDispensers;

    public int levelActiveCrystals = 0, level = 0;
    // Start is called before the first frame update

    [SerializeField] SpriteRenderer numberRenderer;
    [SerializeField] Sprite[] numbers;
    void Awake()
    {
        self = GetComponent<HTNManager>();
    }

    private void Start()
    {
        FadeOutBlack();
    }

    int levelTransitionTimer;
    void FixedUpdate()
    {
        if (levelTransitionTimer < 1 && levelActiveCrystals < 1)
        {
            level += 1;
            levelTransitionTimer = 100;
            foreach (Dispenser disp in activeDispensers)
            {
                disp.ResetPos();
            }
        }

        if (levelTransitionTimer > 0)
        {
            levelTransitionTimer--;

            if (levelTransitionTimer < 1)
            {
                StartLevel();
            }
        }

        HandleRespawn();
        HandleFade();
    }

    void StartLevel()
    {
        int dispenserCount = 0;
        if (level < 4) { dispenserCount = 1; }
        else { dispenserCount = level - 2; }
        if (dispenserCount > dispensers.Length) { dispenserCount = dispensers.Length; }

        int crystalCount = level;

        foreach (Crystal crystal  in crystals)
        {
            Destroy(crystal);
        }
        SetCrystals(crystalCount);
        levelActiveCrystals = crystalCount;

        ActivateDispensers(dispenserCount);

        numberRenderer.sprite = numbers[level % 10];
    }

    public List<int> indices;
    void ActivateDispensers(int count)
    {
        if (level == 1)
        {
            dispensers[0].Initiate(100);
            activeDispensers.Add(dispensers[0]);
            return;
        }

        int fireRatePenalty = 100 - count * 5;
        if (fireRatePenalty < 40) { fireRatePenalty = 40; }

        activeDispensers.Clear();
        indices.Clear();
        for (int i = 0; i < dispensers.Length; i++)
        {
            indices.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            int selectedDisp = GetRandomDispenser();
            dispensers[selectedDisp].Initiate(200 + fireRatePenalty * (count-1));
            activeDispensers.Add(dispensers[selectedDisp]);
        }
    }

    int GetRandomDispenser()
    {
        int ind = Random.Range(0,indices.Count);
        int selected = indices[ind];
        indices.Remove(ind);
        return selected;
    }

    void SetCrystals(int count)
    {
        plate.rotation = Quaternion.identity;
        for (int i = 0; i < count; i++)
        {
            SpawnCrystal(Vector3.forward * arenaSize);
            plate.Rotate(Vector3.up * 360 / count);
        }
        plate.Rotate(Vector3.up * Random.Range(0,360));
    }

    public void SpawnCrystal(Vector3 pos)
    {
        Crystal newCrystal = Instantiate(self.crystal, pos, Quaternion.identity).GetComponent<Crystal>();
        self.crystals.Add(newCrystal);
        newCrystal.id = GetID();
        newCrystal.trfm.parent = plate;
    }

    public static int GetID()
    {
        self.idRegister++;
        return self.idRegister;
    }

    public void GameOver()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        respawnTimer = 0;
        SceneManager.LoadScene("HTN");
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
    
    public void Respawn()
    {
        if (respawnTimer > 0) { return; }
        foreach (Dispenser disp in activeDispensers)
        {
            disp.ResetPos();
        }
        self.FadeToBlack();
        respawnTimer = 150;
    }

    public static int respawnTimer;
    void HandleRespawn()
    {
        if (respawnTimer > 0)
        {
            respawnTimer--;
            if (respawnTimer == 90)
            {
                levelTransitionTimer = 100;
            }
            if (respawnTimer == 50)
            {
                RestartGame();
                FadeOutBlack();
            }
            if (respawnTimer == 0)
            {
                StartLevel();
            }
        }
    }
}
