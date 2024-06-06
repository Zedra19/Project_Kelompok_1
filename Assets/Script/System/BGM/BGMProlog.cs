using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMProlog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM Prolog");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void End()
    {
        AudioManager.Instance.StopMusic("BGM Prolog");
    }
}
