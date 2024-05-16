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
        if (MusicSlider != null || SFXSlider != null)
        {
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        }

        yield return new WaitForSeconds(0.3f);

        MusicVolume();
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
        if (MusicSlider != null || SFXSlider != null)
        {
            AudioManager.Instance.MusicVolume(MusicSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        }
        else
        {
            AudioManager.Instance.MusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        }
    }

    public void SFXVolume()
    {
        if (MusicSlider != null || SFXSlider != null)
        {
            AudioManager.Instance.SFXVolume(SFXSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        }
        else
        {
            AudioManager.Instance.MusicVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }
    }

}
