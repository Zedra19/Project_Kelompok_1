using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    private Coroutine recharge;
    
    public bool Dodge = false;
     
    public Collider characterCollider;

    public float ChargeRate;
    public Image StaminaBar;
    public float CurrentStamina, MaxStamina;
    public float DodgeCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && CurrentStamina >= 1)
        {
            //Dodge = true;
            //characterCollider.enabled = false;
            CurrentStamina -= DodgeCost;
            if(CurrentStamina < 0) CurrentStamina= 0;
            StaminaBar.fillAmount = CurrentStamina / MaxStamina;

            if(recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }
        //else if (Input.GetKeyUp("space"))
        //{
        //    Dodge = false;
        //    characterCollider.enabled = true;
       // }
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
