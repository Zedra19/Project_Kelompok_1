using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public VFX[] EffectVFX;
    public GameObject SourceVFX;

    private void Awake()
    {

    }

    void Start()
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

    void Update()
    {
        
    }

    public void PlayVFXEndless(string name, Vector3 spawnPosition)
    {
        VFX s = Array.Find(EffectVFX, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("VFX Not Found");
        }
        else
        {
            GameObject vfxInstance = GameObject.Instantiate(s.Visual, spawnPosition, Quaternion.identity);
        }
    }

    public void PlayVFX(string name, Vector3 spawnPosition)
    {
        StartCoroutine(VFXOneShot(name, spawnPosition, 2f));
    }

    private IEnumerator VFXOneShot(string name, Vector3 spawnPosition, float delay)
    {
        VFX s = Array.Find(EffectVFX, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("VFX Not Found");
        }
        else
        {
            GameObject vfxInstance = GameObject.Instantiate(s.Visual, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(delay);
            Destroy(vfxInstance);
        }
    }
}
// }