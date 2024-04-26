using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackTrigger : MonoBehaviour
{
    Combo comboScript;
    KillCount killCountScript;
    PlayerAttack playerAttackScript;
    Score scoreScript;
    PatternPatih _patih;

    // Start is called before the first frame update
    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        killCountScript = FindAnyObjectByType<KillCount>();
        playerAttackScript = FindObjectOfType<PlayerAttack>();
        scoreScript = FindObjectOfType<Score>();
        _patih = FindObjectOfType<PatternPatih>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy S") && playerAttackScript.IsAttacking)
        {
            Debug.Log("Hit Enemy S");
            scoreScript.currentScore += scoreScript.EnemyS;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy M") && playerAttackScript.IsAttacking)
        {
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
        else if (other.gameObject.CompareTag("Boss") && playerAttackScript.IsAttacking && _patih.IsStunned)
        {
            Debug.Log("Hit Boss");
            if (_patih != null)
            {
                if (_patih.IsGettingHitInThisHit)
                {
                    return;
                }
                _patih.IsGettingHitInThisHit = true;
                comboScript.comboCount++;
                _patih.HP -= playerAttackScript.PlayerDamage;
                Debug.Log("Boss HP: " + _patih.HP);
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
