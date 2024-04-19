using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class EnemyM : MonoBehaviour
{
    public float Speed = 3f;
    [SerializeField] float distanceToPlayer = 10f;
    [SerializeField] float timeIntervalToMove = 3f;
    public float Gravity = 9.81f;
    bool enableToMove = true;
    bool timerIsRunning = false;
    private CharacterController _characterController;
    public Animator _animator; // Animator component
    public GameObject Spear;
    [SerializeField] float spearForce = 10f;
    [SerializeField] float timeIntervalAnimation = 1.5f;
    [SerializeField] float timeBetweenAttack = 3f;
    float moveTimer = 0f;
    float timerBetweenAttack = 0f;
    float timerAttack = 0f;
    SpearEnemyM spearEnemyMScript;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>(); // Get the Animator component
        spearEnemyMScript = Spear.GetComponent<SpearEnemyM>();
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
            moveTimer += Time.deltaTime;
            enableToMove = false;
            timerBetweenAttack += Time.deltaTime;
            if(timerBetweenAttack >= timeBetweenAttack){
                _animator.SetBool("AttackZone", true);
                timerAttack += Time.deltaTime;
                if(timerAttack >= timeIntervalAnimation){
                    var spearClone = Instantiate(Spear, transform.position + transform.forward, transform.rotation);
                    GameObject player = GameObject.FindWithTag("Player");
                    Vector3 playerPosition = player.transform.position - transform.position;
                    playerPosition.y = 0f;
                    spearClone.transform.position = transform.position + new Vector3(0f, 1.8f, 0f);
                    spearClone.GetComponent<Rigidbody>().AddForce(playerPosition.normalized * spearForce, ForceMode.Impulse);
                    timerBetweenAttack = 0f;
                    timerAttack = 0f;
                    // RaycastHit hit;
                    // if(Physics.Raycast(transform.position, playerPosition, out hit)){
                    //     spearClone.transform.position = hit.point;
                    //     spearClone.GetComponent<Rigidbody>().AddForce(playerPosition.normalized * spearForce, ForceMode.Impulse);
                    //     spearClone.GetComponentInChildren<SpearEnemyM>().isAClone = true;
                    //     timerAttack = 0f;
                    // }
                }
            }
            
            if(moveTimer >= timeIntervalToMove)
            {
                moveTimer = 0f;
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