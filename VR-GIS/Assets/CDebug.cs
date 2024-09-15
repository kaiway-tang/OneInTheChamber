using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CDebug : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI debug;
    static CDebug self;
    string[] messages;
    void Start()
    {
        self = GetComponent<CDebug>();
        messages = new string[5];
    }

    int ptr;
    int timer;
    public static void Log(string msg, int pTime = 50)
    {
        self.timer = pTime;

        self.messages[self.ptr] = msg;

        string finalMsg = "";
        for (int i = 0; i < self.messages.Length; i++)
        {
            finalMsg = self.messages[(self.ptr + i) % self.messages.Length] + '\n';
        }

        self.ptr--;
        if (self.ptr < 0) { self.ptr += self.messages.Length; }

        self.debug.text = finalMsg;

        Debug.Log(msg);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
            if (timer == 0)
            {
                debug.text = "";
            }
        }
    }
}
