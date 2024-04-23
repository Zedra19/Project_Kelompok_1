using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we need to create some variant of character so we can create new script and inherit from here per character
public class Stat : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private PlayerMovement _playerMovement;

    public virtual void UpgradePlayerHealth(int newHealth)
    {
        _playerHealth.maxHealth = newHealth;
        _playerHealth.currentHealth = newHealth;
    }

    public virtual void UpgradePlayerSpeed(float newSpeed)
    {
        _playerMovement.SetSpeed(newSpeed);
    }

    public virtual void UpgradePlayerRunMultiplier(float newRunMultiplier)
    {
        _playerMovement.SetRunMultiplier(newRunMultiplier);
    }

    public virtual void UpgradePlayerDodge(float newDodge)
    {
        _playerMovement.SetDodge(newDodge);
    }

    public virtual void UpgradePlayerDamageArea()
    {

    }
}
