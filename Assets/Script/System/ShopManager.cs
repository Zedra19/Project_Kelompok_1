using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Score _score;

    [SerializeField] private string _buyMovementID = "Movement";
    [SerializeField] private int _movementPrice = 100;
    public UnityEvent BuyMovement;

    [SerializeField] private string _buyDamageID = "Damage";
    [SerializeField] private int _damagePrice = 100;
    public UnityEvent BuyDamage;


    public void BuyUpgrade(string buyID)
    {
        if (buyID == _buyMovementID)
        {
            CheckBuying(_movementPrice, BuyMovement);
        }
        else if (buyID == _buyDamageID)
        {
            CheckBuying(_damagePrice, BuyDamage);
        }
        else
        {
            Debug.Log("Invalid ID!");
        }
    }

    private void CheckBuying(int price, UnityEvent buyEvent)
    {
        if (_score.currentScore >= price)
        {
            _score.currentScore -= price;
            StaticScore.currentScore = _score.currentScore;
            buyEvent?.Invoke();
        }
        else
        {
            Debug.Log("Not enough score!");
        }
    }
}
