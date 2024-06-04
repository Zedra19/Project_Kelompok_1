using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackTriggerPetani : MonoBehaviour
{
    Combo comboScript;
    KillCount killCountScript;
    [SerializeField] private PlayerAttack_Petani playerAttackScript;
    Score scoreScript;
    PatternPatih _patih;
    Senopati senopati;

    private void OnEnable()
    {
        RageMode.OnRageMode += RageModeSizeCube;
    }

    private void OnDisable()
    {
        RageMode.OnRageMode += RageModeSizeCube;
    }
    // Start is called before the first frame update
    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        killCountScript = FindAnyObjectByType<KillCount>();
        scoreScript = FindObjectOfType<Score>();
        _patih = FindObjectOfType<PatternPatih>();
        senopati = FindObjectOfType<Senopati>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerAttackScript.PlayerDamage);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy S") && playerAttackScript.IsAttacking)
        {
            AudioManager.Instance.PlaySFX("Hit");
            Debug.Log("Hit Enemy S");
            scoreScript.currentScore += scoreScript.EnemyS;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy M") && playerAttackScript.IsAttacking)
        {
            AudioManager.Instance.PlaySFX("Hit");
            Debug.Log("Hit Enemy M");
            scoreScript.currentScore += scoreScript.EnemyM;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy L") && playerAttackScript.IsAttacking)
        {

            Debug.Log("Hit Enemy L");
            EnemyL enemyL = other.gameObject.GetComponent<EnemyL>();

            if (enemyL != null)
            {
                //making the L enemy only hitable once per attack
                if (enemyL.IsGettingHitInThisHit)
                {
                    return;
                }
                enemyL.IsGettingHitInThisHit = true;
                AudioManager.Instance.PlaySFX("Hit");
                Debug.Log("Hit Enemy L");
                comboScript.comboCount++;
                enemyL.TakeDamage(playerAttackScript.PlayerDamage);
                if (enemyL.CurrentHealth <= 0)
                {
                    killCountScript.killCount++;
                    scoreScript.currentScore += scoreScript.EnemyL;
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.gameObject.CompareTag("Senopati") && playerAttackScript.IsAttacking)
        {
            Debug.Log("---SENOPATI");
            if (senopati != null)
            {
                Debug.Log("----Hit Senopati HP SENOPATI: " + senopati.HP + "----");
                if (senopati.isGettingHitInThisHit)
                {
                    return;
                }
                senopati.isGettingHitInThisHit = true;
                // comboScript.comboCount++;
                AudioManager.Instance.PlaySFX("Hit");
                senopati.TakeDamage(playerAttackScript.PlayerDamage);
                Debug.Log("Boss HP: " + senopati.HP);
                if (senopati.HP <= 0)
                {
                    // killCountScript.killCount++;
                    // scoreScript.currentScore += scoreScript.EnemyL;
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
                if (_patih.IsGettingHitInThisHit)
                {
                    return;
                }
                _patih.IsGettingHitInThisHit = true;
                AudioManager.Instance.PlaySFX("Hit");
                comboScript.comboCount++;
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

    private void RageModeSizeCube(bool isRageModeOn)
    {
        Debug.Log("Rageee");
        if (isRageModeOn)
        {
            Debug.Log("Scaling bigger");
            transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);

        }
        if (!isRageModeOn)
        {
            Debug.Log("Scaling smaller");
            transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
        }
    }
}
