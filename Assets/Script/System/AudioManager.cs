using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    public Sound[] MusicSounds, SFXSounds;
    public AudioSource MusicSource, SFXSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            MusicSource.clip = s.Clip;
            MusicSource.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            MusicSource.clip = s.Clip;
            MusicSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            SFXSource.PlayOneShot(s.Clip);
        }
    }

    public void ToggleMusic()
    {
        MusicSource.mute = !MusicSource.mute;
    }

    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

    public void MusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
