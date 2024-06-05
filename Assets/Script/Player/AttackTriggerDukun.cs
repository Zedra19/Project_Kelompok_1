using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerDukun : MonoBehaviour
{
    Combo comboScript;
    KillCount killCountScript;
    PlayerAttack_Dukun playerAttackScript;
    Score scoreScript;
    PatternPatih _patih;

    // Dictionary untuk melacak objek yang telah menabrak EnemyL
    Dictionary<GameObject, bool> collidedObjects = new Dictionary<GameObject, bool>();

    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        killCountScript = FindAnyObjectByType<KillCount>();
        playerAttackScript = FindObjectOfType<PlayerAttack_Dukun>();
        scoreScript = FindObjectOfType<Score>();
        _patih = FindObjectOfType<PatternPatih>();
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
    }
}
