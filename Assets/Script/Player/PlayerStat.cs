using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we need to create some variant of character so we can create new script and inherit from here per character
public class PlayerStat : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerAttack_Dukun _playerAttackDukun;

    //updating value based on stat everytime the game start
    private void Awake()
    {
        UpdatePlayerHealth(StaticStat.PlayerHealth);
        UpdatePlayerSpeed(StaticStat.PlayerSpeed);
        UpdatePlayerRunMultiplier(StaticStat.PlayerRunMultiplier);
        UpdatePlayerDodge(StaticStat.PlayerDodge);
        UpdatePlayerDamageLevelStat(StaticStat.PlayerDamageLevelStat);
        UpdatePlayerDamageStat(StaticStat.PlayerDamageStat);
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
        _playerMovement.SetRunMultiplier(newRunMultiplier);
    }

    public virtual void UpdatePlayerDodge(float newDodge)
    {
        _playerMovement.SetDodge(newDodge);
    }

    public virtual void UpdatePlayerDamageLevelStat(int newDamageStat)
    {

    }

    public virtual void UpdatePlayerDamageStat(int newDamageStat)
    {
        GameObject playerKsatria = GameObject.Find("Player-Ksatria");
        GameObject playerDukun = GameObject.Find("Player_Dukun");
        if (playerKsatria != null)
        {
            _playerAttack.PlayerDamage = newDamageStat;
        }
        else if (playerDukun != null)
        {
            _playerAttackDukun.PlayerDamage = newDamageStat;
        }

    }
}
