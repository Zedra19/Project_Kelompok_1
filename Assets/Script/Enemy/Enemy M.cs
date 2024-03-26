using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyM : MonoBehaviour
{
    
    [SerializeField] float speed;
    [SerializeField] float distanceToPlayer;
    private Transform player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > distanceToPlayer + 1)
        {
            // Deketin titik range attack
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else if (distance < distanceToPlayer - 1)
        {
            // Jauhin titik range attack
            Vector3 direction = transform.position - player.position;
            direction.y = 0f;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}