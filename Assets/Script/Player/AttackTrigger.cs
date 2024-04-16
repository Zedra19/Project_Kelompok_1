using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    Combo comboScript;
    KillCount killCountScript;
    PlayerAttack playerAttackScript;
    Score scoreScript;
    // Start is called before the first frame update
    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        killCountScript = FindAnyObjectByType<KillCount>();
        playerAttackScript = FindObjectOfType<PlayerAttack>();
        scoreScript = FindObjectOfType<Score>();
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
            scoreScript.currentScore += scoreScript.EnemyL;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Boss") && playerAttackScript.IsAttacking)
        {
            Debug.Log("Hit Boss");
            scoreScript.currentScore += scoreScript.Boss;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy S") && playerAttackScript.IsAttacking)
        {
            Debug.Log("Hit Enemy S");
            scoreScript.currentScore += scoreScript.EnemyS;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy M") && playerAttackScript.IsAttacking)
        {
            Debug.Log("Hit Enemy M");
            scoreScript.currentScore += scoreScript.EnemyM;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy L") && playerAttackScript.IsAttacking)
        {
            Debug.Log("Hit Enemy L");
            scoreScript.currentScore += scoreScript.EnemyL;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss") && playerAttackScript.IsAttacking)
        {
            Debug.Log("Hit Boss");
            scoreScript.currentScore += scoreScript.Boss;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }
    }
}
