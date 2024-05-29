using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyL : MonoBehaviour
{
    public float AttackRange = 2f; // Set default attack range
    public float KnockbackForce = 5f; // Set default knockback force
    public float CooldownDuration = 2f; // Set default cooldown duration
    public int CurrentHealth = 5;
    public bool IsGettingHitInThisHit;
    [SerializeField] private GameObject attackVFXPrefab;
    [SerializeField] private GameObject heKsatriaVFXPrefab;
    [SerializeField] private GameObject hePrajuritVFXPrefab;

    private string PlayerTag = "Player";
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
        _navMeshAgent.speed = 2.5f;
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

    void ChasePlayer()
    {
        _navMeshAgent.SetDestination(_playerTransform.position);
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    void AttackPlayer()
    {
        AudioManager.Instance.PlaySFX("L Att");
        _animator.SetTrigger("Attack");
        _isAttacking = true;
        GameObject attackVFX = Instantiate(attackVFXPrefab, transform.position, Quaternion.identity);
        Destroy(attackVFX, 2f);
    }

    // Called from animation when the attack is performed
    public void PerformKnockback()
    {
        if (_playerTransform != null)
        {
            Vector3 knockbackDirection = (_playerTransform.position - transform.position).normalized;
            Rigidbody playerRigidbody = _playerTransform.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(knockbackDirection * KnockbackForce, ForceMode.Impulse);
            }

            // Call the TakeDamage method on the player's Health script
            Health playerHealth = _playerTransform.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator StartCooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(CooldownDuration);
        _isOnCooldown = false;
        _isAttacking = false; // Reset attack state after cooldown
    }

    public void TakeDamage(int damage)
    {
        if (_playerTransform != null)
        {
            if (_playerTransform.gameObject.name == "Player-Ksatria(Clone)")
            {
                GameObject heKsatriaVFX = Instantiate(heKsatriaVFXPrefab, transform.position + transform.up * 2, Quaternion.identity);
                heKsatriaVFX.transform.localScale *= 3;
                Destroy(heKsatriaVFX, 2f);
            }
            else if (_playerTransform.gameObject.name == "Player_Prajurit(Clone)")
            {
                GameObject hePrajuritVFX = Instantiate(hePrajuritVFXPrefab, transform.position + transform.up * 2, Quaternion.identity);
                hePrajuritVFX.transform.localScale *= 3;
                Destroy(hePrajuritVFX, 2f);
            }
        }

        if (CurrentHealth > 0)
        {
            Debug.Log($"Enemy L received damage: {damage}");

            CurrentHealth -= damage;
            Debug.Log($"Enemy L is taking damage: {damage}, current health: {CurrentHealth}");

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject, 1f);
        Debug.Log("Enemy L is Dead!");
    }
}
