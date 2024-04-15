using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM Lobby");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UISFX()
    {
        AudioManager.Instance.PlaySFX("UI");
    }

    public Slider MusicSlider, SFXSlider;

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume(float volume)
    {
        AudioManager.Instance.MusicVolume(MusicSlider.value);
    }

    public void SFXVolume(float volume)
    {
        AudioManager.Instance.SFXVolume(SFXSlider.value);
    }
}
