using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RageMode : MonoBehaviour
{
    Combo comboScript;
    [SerializeField] float RageTime = 15f;
    public int ComboRageTrigger;
    public GameObject visualObjectRenderer;
    float rageTimer = 0f;
    public static event Action<bool> OnRageMode;
    private bool _isRaging = false;
    [SerializeField] private GameObject rageModeVFX;


    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        //baseColor = visualObjectRenderer.GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        
    }

    void RageModeFix()
    {
        if (comboScript.comboCount >= ComboRageTrigger)
        {
            //TODO: Replace using vfx instead of change color
            GameObject _rageModeVFX = Instantiate(rageModeVFX, transform.position, Quaternion.identity);
            
            Debug.Log("Rage Mode On");
            // Rage mode Start
            rageTimer += Time.deltaTime;
            //visualObjectRenderer.GetComponent<Renderer>().material.color = RageColor;
            comboScript.comboCount = ComboRageTrigger;
            if (!_isRaging) OnRageMode?.Invoke(true);
            _isRaging = true;
            if (rageTimer >= RageTime)
            {
                // Rage mode ends
                Debug.Log("Rage Mode Off");
                rageTimer = 0;
                comboScript.comboCount = 0;
                //visualObjectRenderer.GetComponent<Renderer>().material.color = baseColor;
                if (_isRaging) OnRageMode?.Invoke(false);
                _isRaging = false;
            }
        }
    }
}
