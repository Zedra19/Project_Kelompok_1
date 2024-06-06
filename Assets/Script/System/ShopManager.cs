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
    [SerializeField] private int _movementPrice = 1000;
    [SerializeField] private string _buyDamageID = "Damage";
    [SerializeField] private int _damagePrice = 1000;
    [SerializeField] private float initialSpeed = 4f;
    [SerializeField] private float maxSpeedBuy = 3f;
    [SerializeField] private int initialDamage = 1;
    [SerializeField] private int maxDamageBuy = 5;
    

    private void Awake()
    {
        
    }

    public void BuyUpgrade(string buyID)
    {
        // Vector3 Player = Player.transform.position;
        if (buyID == _buyMovementID)
        {
            if(StaticStat.PlayerSpeed < (initialSpeed + maxSpeedBuy)){
                CheckBuying(_movementPrice, BuyMovement);
                if (EffectManager != null && Player != null)
                {
                    EffectManager.PlayVFX("Upgrade", Player.transform.position); // if null skip
                }
            }else{
                Debug.LogWarning("Max speed reached!");
            }
        }
        else if (buyID == _buyDamageID)
        {
            if(StaticStat.PlayerDamageStat < (initialDamage + maxDamageBuy)){
                CheckBuying(_damagePrice, BuyDamage);
                if (EffectManager != null && Player != null)
                {
                    EffectManager.PlayVFX("Upgrade", Player.transform.position); // if null skip
                }
            }else{
                Debug.LogWarning("Max damage reached!");
            }
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
