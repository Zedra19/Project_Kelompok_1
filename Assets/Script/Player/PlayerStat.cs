using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we need to create some variant of character so we can create new script and inherit from here per character
public class PlayerStat : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private PlayerMovement _playerMovement;

    //updating value based on stat everytime the game start
    private void Awake()
    {
        UpdatePlayerHealth(StaticStat.PlayerHealth);
        UpdatePlayerSpeed(StaticStat.PlayerSpeed);
        UpdatePlayerRunMultiplier(StaticStat.PlayerRunMultiplier);
        UpdatePlayerDodge(StaticStat.PlayerDodge);
        UpdatePlayerDamagerStat(StaticStat.PlayerDamagerStat);
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

    public virtual void UpgradePlayerDamagerStat(int newDamageStat)
    {
        StaticStat.PlayerDamagerStat += newDamageStat;
        UpdatePlayerDamagerStat(StaticStat.PlayerDamagerStat);
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

    public virtual void UpdatePlayerDamagerStat(int newDamageStat)
    {

    }
}
