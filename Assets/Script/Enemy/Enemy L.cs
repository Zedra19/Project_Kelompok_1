using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyL : MonoBehaviour
{
    public float DetectionRange = 0f;
    public float AttackRange = 0f;
    public float KnockbackForce = 0f;
    public float CooldownDuration = 0f;
    public string PlayerTag = "Player";
    public int CurrentHealth = 50;
    public bool IsGettingHitInThisHit = false;

    private Transform _playerTransform;
    private bool _isOnCooldown = false;
    private bool _isAttacking = false;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private void OnEnable()
    {
        PlayerAttack.OnAttackDone += AllowAttack;
    }

    private void OnDisable()
    {
        PlayerAttack.OnAttackDone -= AllowAttack;
    }

    private void AllowAttack()
    {
        IsGettingHitInThisHit = false;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = AttackRange;
    }

    void Update()
    {
        if (_playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(PlayerTag);

            if (playerObject != null)
            {
                _playerTransform = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Player object not found!");
                return;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= DetectionRange)
        {
            if (distanceToPlayer > AttackRange)
            {
                ChasePlayer();
            }
            else
            {
                if (!_isOnCooldown && !_isAttacking)
                {
                    AttackPlayer();
                }
            }
        }
    }

    void ChasePlayer()
    {
        _navMeshAgent.SetDestination(_playerTransform.position);
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    void AttackPlayer()
    {
        _animator.SetTrigger("Attack");
        _isAttacking = true;
    }

    // Dipanggil dari animasi saat serangan selesai
    public void PerformKnockback()
    {
        Vector3 knockbackDirection = (_playerTransform.position - transform.position).normalized;
        _playerTransform.GetComponent<Rigidbody>().AddForce(knockbackDirection * KnockbackForce, ForceMode.Impulse);

        // Panggil metode TakeDamage pada script Health objek Player
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
        _isAttacking = false; // Setelah cooldown selesai, atur kembali ke false
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Debug.Log("Enemy L is Dead!");
    }
}
