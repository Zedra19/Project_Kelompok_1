using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStage3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM Stage 3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopMusic("BGM Stage 3");
    }
}
