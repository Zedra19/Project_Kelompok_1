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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy S") && playerAttackScript.isAttacking)
        {
            scoreScript.currentScore += scoreScript.EnemyS;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }else if (collision.gameObject.CompareTag("Enemy M") && playerAttackScript.isAttacking)
        {
            scoreScript.currentScore += scoreScript.EnemyM;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }else if (collision.gameObject.CompareTag("Enemy L") && playerAttackScript.isAttacking)
        {
            scoreScript.currentScore += scoreScript.EnemyL;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }else if (collision.gameObject.CompareTag("Boss") && playerAttackScript.isAttacking)
        {
            scoreScript.currentScore += scoreScript.Boss;
            comboScript.comboCount++;
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }
    }
}
