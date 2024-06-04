using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyS : MonoBehaviour
{
    public float Gravity = 9.81f;
    public float AttackRange = 2f;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    public float CooldownDuration = 2f;
    [SerializeField] private GameObject attackVFXPrefab;
    private Transform _playerTransform;
    private bool _isOnCooldown = false;

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
        else
        {
            Debug.LogWarning("Player object not found!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer > AttackRange)
        {
            ChasePlayer();
        }
        else
        {
            if (!_isOnCooldown)
            {
                AttackPlayer();
            }
        }
    }

    void ChasePlayer()
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_playerTransform.position);
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    void AttackPlayer()
    {
        _navMeshAgent.isStopped = true;
        _animator.SetTrigger("Zola Gei");

        GameObject attackVFX = Instantiate(attackVFXPrefab, transform.position, Quaternion.identity);
        Destroy(attackVFX, 2f);

        Health playerHealth = _playerTransform.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }

        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(CooldownDuration);
        _isOnCooldown = false;
    }

    void LateUpdate()
    {
        if (_navMeshAgent.velocity.magnitude > AttackRange)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_navMeshAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _navMeshAgent.angularSpeed);
        }
    }
}
