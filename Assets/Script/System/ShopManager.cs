using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private int _movementPrice = 100;
    [SerializeField] private Score _score;
    public UnityEvent BuyMovement;

    public void BuyUpgrade(string buyID)
    {
        switch (buyID)
        {
            case "Movement":
                CheckBuying(_movementPrice, BuyMovement);
                break;
            default:
                break;
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
