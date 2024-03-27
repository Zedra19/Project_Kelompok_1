using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] healthIcons;

    private bool hasTakenDamage = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (!hasTakenDamage)
        {
            currentHealth -= damage;
            UpdateHealthUI();
            hasTakenDamage = true;

            if (currentHealth <= 0)
            {
                Debug.Log("Player is Dead!");
            }
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < currentHealth)
                healthIcons[i].color = Color.red;
            else
                healthIcons[i].color = Color.white;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hasTakenDamage = false;
        }
    }
}
