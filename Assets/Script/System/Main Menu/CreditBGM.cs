using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMainMenu : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.MusicVolume(0.2f);
        AudioManager.Instance.PlayMusic("BGM Credit");
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopMusic("BGM Credit");
    }
}
