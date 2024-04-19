using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEnemyM : MonoBehaviour
{
    public GameObject enemyM;
    EnemyM enemyMScript;
    public bool isAClone = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyMScript = enemyM.GetComponent<EnemyM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // if(collision.gameObject.CompareTag("Ground") && enemyMScript._animator.GetBool("AttackZone") == true && isAClone == true){
        //     Destroy(gameObject);
        // }
    }
}
