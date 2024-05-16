using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    public Slider MusicSlider, SFXSlider;

    private void Awake()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        MusicVolume();
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        SFXVolume();
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
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(SFXSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

}
