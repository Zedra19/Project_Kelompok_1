using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerAttack
{
    bool IsAttacking { get; }
}

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] healthIcons;
    public Sprite fullHealthSprite;
    public Sprite emptyHealthSprite;
    public IPlayerAttack playerAttackScript; // Menggunakan interface sebagai tipe data
    private bool hasTakenDamage = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        playerAttackScript = GetComponent<IPlayerAttack>(); // Mengambil komponen yang menggunakan interface
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
                healthIcons[i].sprite = fullHealthSprite;
            else
                healthIcons[i].sprite = emptyHealthSprite;
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
        if (collision.gameObject.CompareTag("BossAttack"))
        {
            TakeDamage(1);
        }
    }
}
