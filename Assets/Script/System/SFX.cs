using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void HitSFX()
    {
        AudioManager.Instance.PlaySFX("Hit");
    }

    public void DashSFX()
    {
        AudioManager.Instance.PlaySFX("Dash");
    }
}
