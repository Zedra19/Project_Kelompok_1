using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyS : MonoBehaviour
{
    public float Gravity = 9.81f;
    public float AttackRange = 0.5f;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    public float CooldownDuration = 3f;
    [SerializeField] private GameObject attackVFXPrefab;
    private Transform _playerTransform;
    bool enableToAttack = true;
    bool isAttacking = false;
    float attAnimationTimer = 0f;
    bool hitboxOn = false;
    bool isGettingHit = false;

    bool isOnCooldown = false;
    float timerCooldown = 0f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = AttackRange;
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer > AttackRange)
        {
            ChasePlayer();
        }
        else if(distanceToPlayer <= AttackRange && enableToAttack == true){
            AttackPlayer();
        }

        if(isAttacking && enableToAttack == false){
            transform.rotation = Quaternion.LookRotation(Vector3.zero, transform.position - _playerTransform.position);
            attAnimationTimer += Time.deltaTime;
            if(attAnimationTimer >= 0.6f && attAnimationTimer < 1.5f && isGettingHit == false){
                hitboxOn = true;
            }else if(attAnimationTimer >= 1.5f || isGettingHit == true){
                hitboxOn = false;
                isAttacking = false;
                attAnimationTimer = 0f;
            }
        }else if(isAttacking == false && enableToAttack == false){
            isOnCooldown = true;
        }

        if (isOnCooldown == true){
            timerCooldown += Time.deltaTime;
            if(timerCooldown >= CooldownDuration){
                enableToAttack = true;
                isOnCooldown = false;
                timerCooldown = 0f;
                isGettingHit = false;
            }
        }
    }

    void ChasePlayer()
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_playerTransform.position);
    }

    void AttackPlayer()
    {
        _animator.SetTrigger("Attacking");
        _navMeshAgent.isStopped = true;
        enableToAttack = false;
        isAttacking = true;
        AudioManager.Instance.PlaySFX("S att");
        // GameObject attackVFX = Instantiate(attackVFXPrefab, transform.position, Quaternion.identity);
        // Destroy(attackVFX, 1f);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hitboxOn == true){
            Health playerHealth = _playerTransform.GetComponent<Health>();
            playerHealth.TakeDamage(1);
            isGettingHit = true;
        }
    }
}
