using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;
    private PlayerAttack_Dukun _playerAttackDukun;
    private PlayerAttack_Petani _playerAttackPetani;
    private PlayerAttack_Prajurit _playerAttackPrajurit;

    private void OnEnable()
    {
        BuyUpgradeEvent.OnBuyMovement += UpgradePlayerSpeed;
        BuyUpgradeEvent.OnBuyMovement += UpgradePlayerRunMultiplier;
        BuyUpgradeEvent.OnBuyDamage += UpgradePlayerDamageStat;
    }

    private void OnDisable()
    {
        BuyUpgradeEvent.OnBuyMovement -= UpgradePlayerSpeed;
        BuyUpgradeEvent.OnBuyMovement -= UpgradePlayerRunMultiplier;
        BuyUpgradeEvent.OnBuyDamage -= UpgradePlayerDamageStat;
    }

    private void Awake()
    {
        AssignPlayerComponents();
        UpdatePlayerHealth(StaticStat.PlayerHealth);
        UpdatePlayerSpeed(StaticStat.PlayerSpeed);
        UpdatePlayerRunMultiplier(StaticStat.PlayerRunMultiplier);
        UpdatePlayerDodge(StaticStat.PlayerDodge);
        UpdatePlayerDamageLevelStat(StaticStat.PlayerDamageLevelStat);
        UpdatePlayerDamageStat(StaticStat.PlayerDamageStat);
    }

    private void AssignPlayerComponents()
    {
        GameObject playerKsatria = GameObject.Find("Player-Ksatria(Clone)");
        GameObject playerDukun = GameObject.Find("Player_Dukun(Clone)");
        GameObject playerPetani = GameObject.Find("Player_Petani(Clone)");
        GameObject playerPrajurit = GameObject.Find("Player_Prajurit(Clone)");

        if (playerKsatria != null)
        {
            _playerAttack = playerKsatria.GetComponent<PlayerAttack>();
        }
        if (playerDukun != null)
        {
            _playerAttackDukun = playerDukun.GetComponent<PlayerAttack_Dukun>();
        }
        if (playerPetani != null)
        {
            _playerAttackPetani = playerPetani.GetComponent<PlayerAttack_Petani>();
        }
        if (playerPrajurit != null)
        {
            _playerAttackPrajurit = playerPrajurit.GetComponent<PlayerAttack_Prajurit>();
        }
    }

    public virtual void UpgradePlayerHealth(int addedHealth)
    {
        StaticStat.PlayerHealth += addedHealth;
        UpdatePlayerHealth(StaticStat.PlayerHealth);
    }

    public virtual void UpgradePlayerSpeed(float addedSpeed)
    {
        StaticStat.PlayerSpeed += addedSpeed;
        UpdatePlayerSpeed(StaticStat.PlayerSpeed);
    }

    public virtual void UpgradePlayerRunMultiplier(float addedRunMultiplier)
    {
        StaticStat.PlayerRunMultiplier += addedRunMultiplier;
        UpdatePlayerRunMultiplier(StaticStat.PlayerRunMultiplier);
    }

    public virtual void UpgradePlayerDodge(float addedDodge)
    {
        StaticStat.PlayerDodge += addedDodge;
        UpdatePlayerDodge(StaticStat.PlayerDodge);
    }

    public virtual void UpgradePlayerDamageLevelStat(int newDamageStat)
    {
        StaticStat.PlayerDamageLevelStat += newDamageStat;
        UpdatePlayerDamageLevelStat(StaticStat.PlayerDamageLevelStat);
    }

    public virtual void UpgradePlayerDamageStat(int addedDamageStat)
    {
        StaticStat.PlayerDamageStat += addedDamageStat;
        UpdatePlayerDamageStat(StaticStat.PlayerDamageStat);
    }

    public virtual void UpdatePlayerHealth(int newHealth)
    {
        _playerHealth.maxHealth = newHealth;
        _playerHealth.currentHealth = newHealth;
    }

    public virtual void UpdatePlayerSpeed(float newSpeed)
    {
        _playerMovement.SetSpeed(newSpeed);
    }

    public virtual void UpdatePlayerRunMultiplier(float newRunMultiplier)
    {
        _playerMovement.SetRunMultiplier(newRunMultiplier * 2);
    }

    public virtual void UpdatePlayerDodge(float newDodge)
    {
        _playerMovement.SetDodge(newDodge);
    }

    public virtual void UpdatePlayerDamageLevelStat(int newDamageStat)
    {
        // Logic for updating player damage level stat, if needed
    }

    public virtual void UpdatePlayerDamageStat(int newDamageStat)
    {

        if (_playerAttack != null)
        {
            Debug.Log("UpdatePlayerDamageStat PlayerAttack");
            _playerAttack.PlayerDamage = newDamageStat;
        }
        if (_playerAttackDukun != null)
        {
            Debug.Log("UpdatePlayerDamageStat PlayerAttackDukun");
            _playerAttackDukun.PlayerDamage = newDamageStat;
        }
        if (_playerAttackPetani != null)
        {
            Debug.Log("UpdatePlayerDamageStat PlayerAttackPetani");
            _playerAttackPetani.PlayerDamage = newDamageStat;
        }
        if (_playerAttackPrajurit != null)
        {
            Debug.Log("UpdatePlayerDamageStat PlayerAttackPrajurit");
            _playerAttackPrajurit.PlayerDamage = newDamageStat;
        }
    }
}
