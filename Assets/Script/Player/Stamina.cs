using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    private Coroutine recharge;
     
    public Collider characterCollider;

    public float ChargeRate;
    public Image StaminaBar;
    public float CurrentStamina, MaxStamina;
    public float DodgeCost;
   
    void Start()
    {
        
    }

 
    void Update()
    {
        if (Input.GetKeyDown("space") && CurrentStamina >= 1)
        {
            CurrentStamina -= DodgeCost;
            if(CurrentStamina < 0) CurrentStamina= 0;
            StaminaBar.fillAmount = CurrentStamina / MaxStamina;

            if(recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(.33f);

        while(CurrentStamina < MaxStamina)
        {
            CurrentStamina += ChargeRate / 10f;
            if(CurrentStamina > MaxStamina) CurrentStamina = MaxStamina;
            StaminaBar.fillAmount = CurrentStamina / MaxStamina;
            yield return new WaitForSeconds(.1f);
        }
    }
}
