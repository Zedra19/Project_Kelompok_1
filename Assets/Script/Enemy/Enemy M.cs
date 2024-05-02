using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyM : MonoBehaviour
{
    public float distanceToPlayer = 10f;
    public float timeIntervalToMove = 3f;
    public float Gravity = 9.81f;
    bool enableToMove = true;
    bool timerIsRunning = false;
    public Animator _animator; // Animator component
    public GameObject Spear;
    [SerializeField] float spearForce = 10f;
    [SerializeField] float timeIntervalAnimation = 1.5f;
    [SerializeField] float timeBetweenAttack = 3f;
    float moveTimer = 0f;
    float timerBetweenAttack = 0f;
    float timerAttack = 0f;

    private NavMeshAgent navAgent;
    private GameObject player;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>(); // Get the Animator component
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        moveToAttackRange();
        RotateToPlayer();
        attacking();
    }

    void moveToAttackRange()
{
    if (player != null)
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (!timerIsRunning) // Jika tidak sedang dalam fase serangan
        {
            if (distance > distanceToPlayer + 1 && enableToMove)
            {
                // Deketin titik range attack
                _animator.SetBool("Moving", true); // Mengaktifkan animasi lari
                navAgent.SetDestination(player.transform.position);
            }
            else if (distance < distanceToPlayer - 1 && enableToMove)
            {
                // Jauhin Player
                _animator.SetBool("Moving", true); // Mengaktifkan animasi lari
                navAgent.SetDestination(transform.position - player.transform.position);
            }
            else if (distance <= distanceToPlayer + 1 && distance >= distanceToPlayer - 1)
            {
                _animator.SetBool("Moving", false); // Menonaktifkan animasi lari
                navAgent.ResetPath(); // Menghentikan NavMeshAgent
                timerIsRunning = true;
            }
        }
    }
}


    void attacking()
    {
        if (timerIsRunning)
        {
            moveTimer += Time.deltaTime;
            enableToMove = false;
            timerBetweenAttack += Time.deltaTime;
            if (timerBetweenAttack >= timeBetweenAttack)
            {
                _animator.SetBool("AttackZone", true);
                timerAttack += Time.deltaTime;
                if (timerAttack >= timeIntervalAnimation)
                {
                    var spearClone = Instantiate(Spear, transform.position + transform.forward, transform.rotation);
                    GameObject player = GameObject.FindWithTag("Player");
                    Vector3 playerPosition = player.transform.position - transform.position;
                    playerPosition.y = 0f;
                    spearClone.tag = "Spear";
                    spearClone.transform.position = transform.position + new Vector3(0f, 1.8f, 0f);
                    spearClone.GetComponent<Rigidbody>().AddForce(playerPosition.normalized * spearForce, ForceMode.Impulse);
                    timerBetweenAttack = 0f;
                    timerAttack = 0f;
                }
            }

            if (moveTimer >= timeIntervalToMove)
            {
                moveTimer = 0f;
                enableToMove = true;
                timerIsRunning = false;
                _animator.SetBool("AttackZone", false);
            }
        }
    }

    void RotateToPlayer()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position - transform.position;
            playerPosition.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(playerPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
