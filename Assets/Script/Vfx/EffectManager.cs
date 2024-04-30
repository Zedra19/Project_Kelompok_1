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
    public GameObject SourceVFX;
    // public List<GameObject> effects;
    // public List<KeyCode> keys;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // for (int i = 0; i < keys.Count; i++)
        // {
        //     if (Input.GetKeyDown(keys[i]))
        //     {
        //         Instantiate<GameObject>(effects[i]);
        //     }
        // }
    }

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

    public void PlayVFX(string name, Vector3 spawnPosition)
    {
        VFX s = Array.Find(EffectVFX, x => x.Name == name);

        if(s == null)
        {
            Debug.Log("VFX Not Found");
        }
        else
        {
            GameObject.Instantiate(SourceVFX, spawnPosition, Quaternion.identity);
        }
    }
}
// }