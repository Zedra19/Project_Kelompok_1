using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RageMode : MonoBehaviour
{
    Combo comboScript;
    [SerializeField] float RageTime = 15f;
    public int ComboRageTrigger;
    // public GameObject visualObjectRenderer;
    float rageTimer = 0f;
    public static event Action<bool> OnRageMode;
    private bool _isRaging = false;
    [SerializeField] private GameObject rageModeVFX;
    private GameObject rageModeInstance;
    private bool rageModeInstanceCreated = false;


    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        //baseColor = visualObjectRenderer.GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Rage();
        }

        RageModeFix();
    }

    void RageModeFix()
    {
        if (comboScript.comboCount >= ComboRageTrigger)
        {

            Rage();
        }
    }

    private void Rage()
    {
        //TODO: Replace using vfx instead of change color
        if (!rageModeInstanceCreated)
        {
            rageModeInstance = Instantiate(rageModeVFX, transform.position, Quaternion.identity);
            rageModeInstance.transform.SetParent(transform);

            rageModeInstanceCreated = true;
        }
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
            Destroy(rageModeInstance);
            rageModeInstanceCreated = false;
            rageTimer = 0;
            comboScript.comboCount = 0;
            //visualObjectRenderer.GetComponent<Renderer>().material.color = baseColor;
            if (_isRaging) OnRageMode?.Invoke(false);
            _isRaging = false;
        }
    }
}
