using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuyUpgradeEvent : MonoBehaviour
{
    [SerializeField] private float _movementAmount = 1f;
    [SerializeField] private int _damageAmount = 1;

    public static event Action<float> OnBuyMovement;
    public static event Action<int> OnBuyDamage;

    public void BuyMovement()
    {
        OnBuyMovement?.Invoke(_movementAmount);
    }

    public void BuyDamage()
    {
        OnBuyDamage?.Invoke(_damageAmount);
    }
}
