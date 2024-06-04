using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Slider BossHealthSlider;
    public int BossMaxHealth = 5; // Initial max health of the boss
    private int _bossCurrentHealth; // Current health of the boss

    private void Start()
    {
        _bossCurrentHealth = BossMaxHealth;
        SetMaxHealth(BossMaxHealth);
    }

    public void SetMaxHealth(int health)
    {
        BossHealthSlider.maxValue = health;
        BossHealthSlider.value = health;
    }

    public void SetHealth(int health)
    {
        BossHealthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        _bossCurrentHealth -= damage;
        SetHealth(_bossCurrentHealth);

        // Check if the boss is defeated
        if (_bossCurrentHealth <= 0)
        {
            Debug.Log("Boss defeated!");
        }
    }
}
