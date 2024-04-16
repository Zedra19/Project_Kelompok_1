using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyM : MonoBehaviour
{
    public float Speed = 3f;
    [SerializeField] float distanceToPlayer = 10f;
    private float timer = 0f;
    [SerializeField] float timeIntervalToMove = 3f;
    public float Gravity = 9.81f;
    bool enableToMove = true;
    bool timerIsRunning = false;
    private CharacterController _characterController;
    private Animator _animator; // Animator component

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        moveToAttackRange();
        RotateToPlayer();
        attacking();
    }

    void moveToAttackRange(){
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > distanceToPlayer + 1 && enableToMove){
                // Deketin titik range attack
                Vector3 direction = player.transform.position - transform.position;
                direction.Normalize();
                direction.y = 0f;
                Vector3 movement = direction * Speed * Time.deltaTime;
                movement.y -= Gravity * Time.deltaTime;
                _characterController.Move(movement);
                _animator.SetBool("Moving", true);
            }
            else if (distance < distanceToPlayer - 1 && enableToMove){
                // Jauhin Player
                Vector3 direction = transform.position - player.transform.position;
                direction.Normalize();
                direction.y = 0f;
                Vector3 movement = direction * Speed * Time.deltaTime;
                movement.y -= Gravity * Time.deltaTime;
                _characterController.Move(movement);
                _animator.SetBool("Moving", true);
            }else if(distance <= distanceToPlayer + 1 && distance >= distanceToPlayer - 1){
                _animator.SetBool("Moving",false);
                timerIsRunning = true;
            }
        }
    }

    void attacking(){
        if(timerIsRunning)
        {
            timer += Time.deltaTime;
            enableToMove = false;
            _animator.SetBool("AttackZone", true);
            if(timer >= timeIntervalToMove)
            {
                timer = 0f;
                enableToMove = true;
                timerIsRunning = false;
                _animator.SetBool("AttackZone", false);
            }
        }
    }

    void RotateToPlayer(){
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPosition = player.transform.position - transform.position;
        playerPosition.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(playerPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}