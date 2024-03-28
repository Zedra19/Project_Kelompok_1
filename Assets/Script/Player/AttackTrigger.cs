using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    Combo comboScript;
    KillCount killCountScript;
    PlayerAttack playerAttackScript;
    // Start is called before the first frame update
    void Start()
    {
        comboScript = FindObjectOfType<Combo>();
        killCountScript = FindAnyObjectByType<KillCount>();
        playerAttackScript = FindObjectOfType<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && playerAttackScript.isAttacking)
        {
            comboScript.comboCount++;
            Destroy(collision.gameObject);
            killCountScript.killCount++;
            Destroy(collision.gameObject);
        }
    }
}
