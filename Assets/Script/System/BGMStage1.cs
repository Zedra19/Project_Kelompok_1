using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStage1 : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM Stage 1");
    }

    void Update()
    {
        
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopMusic("BGM Stage 1");
    }
}
