using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RageMode : MonoBehaviour
{
    Combo comboScript;
    [SerializeField] float RageTime;
    public int ComboRageTrigger;
    public GameObject visualObjectRenderer;
    float rageTimer = 0f;
    Color baseColor;
    Color RageColor = Color.red; //ganti warna rage mode sesuai keinginan
    public static event Action<bool> OnRageMode;
    private bool _isRaging = false;


    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        baseColor = visualObjectRenderer.GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if (comboScript.comboCount >= ComboRageTrigger)
        {
            //TODO: Replace using vfx instead of change color
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
