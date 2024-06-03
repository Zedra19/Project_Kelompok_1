using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStageEndless : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM Endless");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopMusic("BGM Endless");
    }
}
