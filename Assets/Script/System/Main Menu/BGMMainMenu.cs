using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMenu : MonoBehaviour
{
    public void Start()
    {
        AudioManager.Instance.MusicVolume(0.2f);
        AudioManager.Instance.PlayMusic("BGM Lobby");
    }

    public void StopBGMLobby()
    {
        AudioManager.Instance.StopMusic("BGM Lobby");
    }
}
