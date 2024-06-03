using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditBGM : MonoBehaviour
{
    public void Start()
    {
        AudioManager.Instance.PlayMusic("BGM Credit");
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopMusic("BGM Credit");
        AudioManager.Instance.PlayMusic("BGM Lobby");
    }
}
