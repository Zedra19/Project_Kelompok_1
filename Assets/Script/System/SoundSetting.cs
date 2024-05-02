using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    public Slider MusicSlider, SFXSlider;

    void Start()
    {
        AudioManager.Instance.MusicVolume(0.2f);
        AudioManager.Instance.PlayMusic("BGM Lobby");
    }

    public void UISFX()
    {
        AudioManager.Instance.PlaySFX("UI");
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(MusicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(SFXSlider.value);
    }

    public void StopBGMLobby()
    {
        AudioManager.Instance.StopMusic("BGM Lobby");
    }
}
