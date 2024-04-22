using System.Collections;
using UnityEngine;

public class EnemyL : MonoBehaviour
{
    public int maxHealth = 0; 
    public int currentHealth; 
    public float DetectionRange = 0f;
    public float AttackRange = 0f;
    public float ChaseSpeed = 0f;
    public float KnockbackForce = 0f;
    public float CooldownDuration = 0f;
    public string PlayerTag = "Player";

    private Transform _playerTransform;
    private bool _isOnCooldown = false;
    private bool _isAttacking = false;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
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

        // Mengatur rotasi objek agar selalu menghadap ke arah gerakan
        if (_playerTransform != null)
        {
            Vector3 lookDirection = _playerTransform.position - transform.position;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    void ChasePlayer()
    {
        Vector3 moveDirection = (_playerTransform.position - transform.position).normalized;
        transform.position += moveDirection * ChaseSpeed * Time.deltaTime;

        // Memainkan animasi berjalan
        _animator.SetFloat("Speed", ChaseSpeed);
    }

    void AttackPlayer()
    {
        // Memainkan animasi serangan
        _animator.SetTrigger("Attack");
        _isAttacking = true;
    }

    // Dipanggil dari animasi saat serangan selesai
    public void PerformKnockback()
    {
        Vector3 knockbackDirection = (_playerTransform.position - transform.position).normalized;
        _playerTransform.GetComponent<Rigidbody>().AddForce(knockbackDirection * KnockbackForce, ForceMode.Impulse);
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
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
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
