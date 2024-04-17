using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    public GameObject MovementPop;
    public GameObject DodgePop;
    public GameObject AttPop;
    public GameObject Health;
    public GameObject HealthPop;
    public GameObject Stamina;
    public GameObject StaminaPop;
    public GameObject Stats;
    public GameObject StatsPop;

    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;
    private bool mouseMoved = false;
    private bool leftMouseClicked = false;
    private bool dodgePopDisplayed = false;
    private bool healthPopDisplayed = false;
    private bool staminaPopDisplayed = false;
    private bool statsPopDisplayed = false;

    private bool movementInstructionCompleted = false;
    private bool dodgeInstructionCompleted = false;
    private bool attInstructionCompleted = false;
    private bool healthInstructionCompleted = false;
    private bool staminaInstructionCompleted = false;
    private bool statsInstructionCompleted = false;

    void Update()
    {
        if (!movementInstructionCompleted)
            MovementInstruction();
        else if (!dodgeInstructionCompleted)
            DodgeInstruction();
        else if (!attInstructionCompleted)
            AttInstruction();
        else if (!healthInstructionCompleted)
            HealthInstruction();
        else if (!staminaInstructionCompleted)
            StaminaInstruction();
        else if (!statsInstructionCompleted)
            StatsInstruction();
    }

    void MovementInstruction()
    {
        if (!wPressed && Input.GetKeyDown(KeyCode.W))
        {
            wPressed = true;
        }
        if (!aPressed && Input.GetKeyDown(KeyCode.A))
        {
            aPressed = true;
        }
        if (!sPressed && Input.GetKeyDown(KeyCode.S))
        {
            sPressed = true;
        }
        if (!dPressed && Input.GetKeyDown(KeyCode.D))
        {
            dPressed = true;
        }

        if (wPressed && aPressed && sPressed && dPressed)
        {
            MovementPop.SetActive(false);
            if (!dodgePopDisplayed) // Hanya munculkan "Dodge" pop-up jika belum ditampilkan sebelumnya
            {
                DodgePop.SetActive(true);
                movementInstructionCompleted = true;
            }
        }
    }

    void DodgeInstruction()
    {
        if (wPressed && aPressed && sPressed && dPressed && Input.GetKeyDown(KeyCode.Space))
        {
            DodgePop.SetActive(false);
            dodgePopDisplayed = true; // Tandai bahwa "Dodge" pop-up sudah ditampilkan
            AttPop.SetActive(true);
            dodgeInstructionCompleted = true;
        }
    }

    void AttInstruction()
    {
        if (wPressed && aPressed && sPressed && dPressed && !mouseMoved && Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Mouse Y") != 0)
        {
            mouseMoved = true;
        }

        if (wPressed && aPressed && sPressed && dPressed && !leftMouseClicked && Input.GetMouseButtonDown(0))
        {
            leftMouseClicked = true;
        }

        if (wPressed && aPressed && sPressed && dPressed && mouseMoved && leftMouseClicked)
        {
            AttPop.SetActive(false);
            if (!healthPopDisplayed) // Hanya munculkan pop-up kesehatan jika belum ditampilkan sebelumnya
            {
                Health.SetActive(true);
                HealthPop.SetActive(true);
                healthPopDisplayed = true;
                attInstructionCompleted = true;
            }
        }
    }

    void HealthInstruction()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !staminaPopDisplayed)
        {
            staminaPopDisplayed = true;
            HealthPop.SetActive(false);
            Stamina.SetActive(true);
            StaminaPop.SetActive(true);
            healthInstructionCompleted = true;
        }
    }

    void StaminaInstruction()
    {
        if (staminaPopDisplayed && Input.GetKeyDown(KeyCode.Return) && !statsPopDisplayed)
        {
            statsPopDisplayed = true;
            StaminaPop.SetActive(false);
            Stats.SetActive(true);
            StatsPop.SetActive(true);
            staminaInstructionCompleted = true;
        }
    }

    void StatsInstruction()
    {
        if (statsPopDisplayed && Input.GetKeyDown(KeyCode.Return))
        {
            StatsPop.SetActive(false);
            statsInstructionCompleted = true;
        }
    }
}
