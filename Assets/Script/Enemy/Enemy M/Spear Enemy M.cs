using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearEnemyM : MonoBehaviour
{
    public GameObject enemyM;
    EnemyM enemyMScript;
    // Start is called before the first frame update
    void Start()
    {
        enemyMScript = enemyM.GetComponent<EnemyM>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != "Spear")
        {
            transform.position = gameObject.transform.parent.position;
            transform.eulerAngles = gameObject.transform.parent.eulerAngles + new Vector3(0,90f,0);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (gameObject.tag == "Spear" && collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}