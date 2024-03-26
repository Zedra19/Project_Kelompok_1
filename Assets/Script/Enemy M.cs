using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;

public class EnemyM : MonoBehaviour
{
    
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    private Transform player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stoppingDistance + 1)
        {
            // Move towards the player
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else if (distance < stoppingDistance - 1)
        {
            // Move towards the player
            Vector3 direction = transform.position - player.position;
            direction.y = 0f;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}