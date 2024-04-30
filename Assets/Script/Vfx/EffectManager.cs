using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// namespace EffectSceneManager
// {
public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public VFX[] EffectVFX;

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    public void PlayVFX(string name, Vector3 spawnPosition)
    {
        VFX s = Array.Find(EffectVFX, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("VFX Not Found");
        }
        else
        {
            GameObject.Instantiate(s.Visual, spawnPosition, Quaternion.identity);
        }
    }
}
// }