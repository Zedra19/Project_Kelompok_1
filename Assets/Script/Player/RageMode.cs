using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMode : MonoBehaviour
{
    Combo comboScript;
    [SerializeField] float RageTime;
    public int ComboRageTrigger;
    float timer = 0f;
    Color baseColor;
    Color RageColor = Color.red; //ganti warna rage mode sesuai keinginan
    

    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        baseColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if(comboScript.comboCount >= ComboRageTrigger)
        {
            // Rage mode Start
            timer += Time.deltaTime;
            GetComponent<Renderer>().material.color = RageColor;
            comboScript.comboCount = ComboRageTrigger;
            if(timer >= RageTime)
            {
                // Rage mode ends
                timer = 0;
                comboScript.comboCount = 0;
                GetComponent<Renderer>().material.color = baseColor;
            }
        }
    }
}