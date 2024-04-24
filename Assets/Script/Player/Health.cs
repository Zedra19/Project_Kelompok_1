using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] healthIcons;
    public PlayerAttack playerAttackScipt;

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

        // Tambahkan baris berikut untuk mereset hasTakenDamage setelah jangka waktu tertentu
        StartCoroutine(ResetDamageFlag());
    }

    IEnumerator ResetDamageFlag()
    {
        yield return new WaitForSeconds(0.5f); // Ubah waktu sesuai kebutuhan
        hasTakenDamage = false;
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
        if (collision.gameObject.CompareTag("Spear"))
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy S") || collision.gameObject.CompareTag("Spear") || collision.gameObject.CompareTag("Enemy L") || collision.gameObject.CompareTag("Boss"))
        {
            hasTakenDamage = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy S"))
        {
            if (playerAttackScipt.IsAttacking)
            {
                TakeDamage(0);
            }
            else
            {
                TakeDamage(1);
            }

        }
        else if (collision.gameObject.CompareTag("BossAttack"))
        {
            TakeDamage(1);
        }
    }
}
