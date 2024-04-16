using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] private float waitToDamaging = 0.5f;
    private Combo comboScript;
    private KillCount killCountScript;
    private Score scoreScript;
    private bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        killCountScript = FindAnyObjectByType<KillCount>();
        scoreScript = FindObjectOfType<Score>();
    }

    private void OnEnable()
    {
        PlayerAttack.OnAttackDamaging += OnAttack;

    }

    private void OnDisable()
    {
        PlayerAttack.OnAttackDamaging -= OnAttack;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnAttack(bool start)
    {
        if (start)
        {
            StartCoroutine(HitAtTheTime());
        }
        else
        {
            isAttacking = false;
        }
    }

    IEnumerator HitAtTheTime()
    {
        yield return new WaitForSeconds(waitToDamaging);
        isAttacking = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking)
        {
            if (other.gameObject.CompareTag("Enemy S"))
            {
                Debug.Log("Hit Enemy S");
                DestroyEnemy(scoreScript.EnemyS, other.gameObject);
            }
            else if (other.gameObject.CompareTag("Enemy M"))
            {
                Debug.Log("Hit Enemy M");
                DestroyEnemy(scoreScript.EnemyM, other.gameObject);
            }
            else if (other.gameObject.CompareTag("Enemy L"))
            {
                Debug.Log("Hit Enemy L");
                DestroyEnemy(scoreScript.EnemyL, other.gameObject);
            }
            else if (other.gameObject.CompareTag("Boss"))
            {
                Debug.Log("Hit Boss");
                DestroyEnemy(scoreScript.Boss, other.gameObject);
            }
        }

    }

    void DestroyEnemy(int score, GameObject other)
    {
        scoreScript.currentScore += score;
        comboScript.comboCount++;
        killCountScript.killCount++;
        Destroy(other);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Enemy S") && playerAttackScript.IsAttacking)
    //     {
    //         Debug.Log("Hit Enemy S");
    //         scoreScript.currentScore += scoreScript.EnemyS;
    //         comboScript.comboCount++;
    //         killCountScript.killCount++;
    //         Destroy(collision.gameObject);
    //     }
    //     else if (collision.gameObject.CompareTag("Enemy M") && playerAttackScript.IsAttacking)
    //     {
    //         Debug.Log("Hit Enemy M");
    //         scoreScript.currentScore += scoreScript.EnemyM;
    //         comboScript.comboCount++;
    //         killCountScript.killCount++;
    //         Destroy(collision.gameObject);
    //     }
    //     else if (collision.gameObject.CompareTag("Enemy L") && playerAttackScript.IsAttacking)
    //     {
    //         Debug.Log("Hit Enemy L");
    //         scoreScript.currentScore += scoreScript.EnemyL;
    //         comboScript.comboCount++;
    //         killCountScript.killCount++;
    //         Destroy(collision.gameObject);
    //     }
    //     else if (collision.gameObject.CompareTag("Boss") && playerAttackScript.IsAttacking)
    //     {
    //         Debug.Log("Hit Boss");
    //         scoreScript.currentScore += scoreScript.Boss;
    //         comboScript.comboCount++;
    //         killCountScript.killCount++;
    //         Destroy(collision.gameObject);
    //     }
    // }
}
