using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerPrajurit : MonoBehaviour
{
    Combo comboScript;
    KillCount killCountScript;
    PlayerAttack_Prajurit playerAttackScript;
    Score scoreScript;
    PatternPatih _patih;
    Senopati senopati;

    // Dictionary untuk melacak objek yang telah menabrak EnemyL
    Dictionary<GameObject, bool> collidedObjects = new Dictionary<GameObject, bool>();

    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        killCountScript = FindAnyObjectByType<KillCount>();
        playerAttackScript = FindObjectOfType<PlayerAttack_Prajurit>();
        scoreScript = FindObjectOfType<Score>();
        _patih = FindObjectOfType<PatternPatih>();
        senopati = FindObjectOfType<Senopati>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy L"))
        {
            // Tambahkan objek yang menabrak EnemyL ke dalam dictionary
            if (!collidedObjects.ContainsKey(other.gameObject))
            {
                collidedObjects.Add(other.gameObject, false);
            }
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy S") && playerAttackScript.IsAttacking)
        {
            // Penanganan ketika EnemyS terkena serangan
            Debug.Log("Hit Enemy S");
            scoreScript.currentScore += scoreScript.EnemyS;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy M") && playerAttackScript.IsAttacking)
        {
            // Penanganan ketika EnemyM terkena serangan
            Debug.Log("Hit Enemy M");
            scoreScript.currentScore += scoreScript.EnemyM;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy L") && playerAttackScript.IsAttacking)
        {
            EnemyL enemyL = other.gameObject.GetComponent<EnemyL>();

            if (enemyL != null)
            {
                if (!collidedObjects.ContainsKey(other.gameObject) || collidedObjects[other.gameObject])
                {
                    // Jika objek belum ada dalam dictionary atau sudah terkena damage, lewati
                    return;
                }

                Debug.Log("Hit Enemy L");
                comboScript.comboCount++;
                enemyL.TakeDamage(playerAttackScript.PlayerDamage);

                // Tandai objek sebagai sudah terkena damage
                collidedObjects[other.gameObject] = true;

                if (enemyL.CurrentHealth <= 0)
                {
                    killCountScript.killCount++;
                    scoreScript.currentScore += scoreScript.EnemyL;
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.gameObject.CompareTag("Patih") && playerAttackScript.IsAttacking && _patih.IsStunned)
        {
            Debug.Log("---PATIH");
            if (_patih != null)
            {
                Debug.Log("----Hit Patih HP PATIH: " + _patih.HP + "----");
                Debug.Log($"_patih.IsGettingHitInThisHit: {_patih.IsGettingHitInThisHit}");
                if (_patih.IsGettingHitInThisHit)
                {
                    return;
                }
                _patih.IsGettingHitInThisHit = true;
                comboScript.comboCount++;
                AudioManager.Instance.PlaySFX("Hit");
                Debug.Log("Player Damage: " + playerAttackScript.PlayerDamage);
                _patih.HP -= playerAttackScript.PlayerDamage;
                Debug.Log("Patih HP: " + _patih.HP);
                if (_patih.HP <= 0)
                {
                    killCountScript.killCount++;
                    scoreScript.currentScore += scoreScript.Boss;
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.gameObject.CompareTag("Senopati") && playerAttackScript.IsAttacking)
        {
            Debug.Log("---SENOPATI");
            if (senopati != null)
            {
                Debug.Log($"senopati.isGettingHitInThisHit{senopati.isGettingHitInThisHit}");
                if (senopati.isGettingHitInThisHit)
                {
                    Debug.Log("Senopati is getting hit in this hit");
                    return;
                }
                senopati.isGettingHitInThisHit = true;

                // comboScript.comboCount++;
                AudioManager.Instance.PlaySFX("Hit");
                Debug.Log("Player Damage: " + playerAttackScript.PlayerDamage);
                senopati.TakeDamage(playerAttackScript.PlayerDamage);
                Debug.Log("Boss Senopati HP: " + senopati.HP);
                if (senopati.GetCurrentHealth() <= 0)
                {
                    // killCountScript.killCount++;
                    // scoreScript.currentScore += scoreScript.EnemyL;
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
