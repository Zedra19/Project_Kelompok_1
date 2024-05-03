using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public void HitSFX()
    {
        AudioManager.Instance.PlaySFX("Hit");
    }

    public void DashSFX()
    {
        AudioManager.Instance.PlaySFX("Dash");
    }
}
