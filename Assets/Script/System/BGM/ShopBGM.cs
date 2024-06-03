using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("Shop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopMusic("Shop");
    }
}
