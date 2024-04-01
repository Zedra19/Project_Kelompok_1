using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyM : MonoBehaviour
{
    
    [SerializeField] float speed;
    [SerializeField] float distanceToPlayer;
    private Transform player;
    private float timer = 0f;
    [SerializeField] float timeIntervalToMove;
    bool enableToMove = true;
    bool timerIsRunning = false;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        moving();
        timerCount();
    }

    void moving(){
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > distanceToPlayer + 1 && enableToMove)
        {
            // Deketin titik range attack
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else if (distance < distanceToPlayer - 1 && enableToMove)
        {
            // Jauhin titik range attack
            Vector3 direction = transform.position - player.position;
            direction.y = 0f;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else if(distance <= distanceToPlayer + 1 && distance >= distanceToPlayer - 1)
        {
            timerIsRunning = true;
        }
    }

    void timerCount(){
        if(timerIsRunning)
        {
            timer += Time.deltaTime;
            enableToMove = false;
            if(timer >= timeIntervalToMove)
            {
                timer = 0f;
                enableToMove = true;
                timerIsRunning = false;
            }
        }
    }
}