using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    public EffectManager EffectManager;
    public GameObject Player;
    public UnityEvent BuyMovement;
    public UnityEvent BuyDamage;

    [SerializeField] private Score _score;
    [SerializeField] private string _buyMovementID = "Movement";
    [SerializeField] private int _movementPrice = 100;
    [SerializeField] private string _buyDamageID = "Damage";
    [SerializeField] private int _damagePrice = 100;
    
    public void BuyUpgrade(string buyID)
    {
        // Vector3 Player = Player.transform.position;
        if (buyID == _buyMovementID)
        {
            CheckBuying(_movementPrice, BuyMovement);
            EffectManager.PlayVFX("Upgrade", Player.transform.position);
        }
        else if (buyID == _buyDamageID)
        {
            CheckBuying(_damagePrice, BuyDamage);
            EffectManager.PlayVFX("Upgrade", Player.transform.position);
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
