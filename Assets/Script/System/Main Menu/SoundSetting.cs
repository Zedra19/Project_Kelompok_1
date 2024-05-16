using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundSetting : MonoBehaviour
{
    public Slider MusicSlider, SFXSlider;

    private void Awake()
    {
        StartCoroutine(ProcessAwake());
    }
    private IEnumerator ProcessAwake()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        yield return new WaitForSeconds(0.3f);
    }

    void Update(){
        AudioManager.Instance.MusicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        AudioManager.Instance.SFXSource.volume = PlayerPrefs.GetFloat("SFXVolume");
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
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
    }

    public void SFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }

}
