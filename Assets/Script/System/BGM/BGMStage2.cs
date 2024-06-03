using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStage2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM Stage 2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopMusic("BGM Stage 2");
    }
}
