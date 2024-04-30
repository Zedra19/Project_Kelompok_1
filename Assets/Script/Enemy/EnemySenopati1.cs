using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Senopati : MonoBehaviour
{
    public Transform player;
    public Transform[] hitboxSpawnPoints;
    public GameObject hitboxPrefab;
    public GameObject rageHitboxPrefab;
    public Transform attackSpawnPoint;
    public BossHealth bossHealth;

    public float detectionRange = 10000000000f;
    public float attackRange = 6f;
    public float retreatRange = 3f;
    public float chaseSpeed = 5f;
    public float hitboxSpeed = 10f;
    public float hitboxDuration = 1f;
    public float rageDuration = 20f;
    public float attackCooldown = 4f;
    public float HP;
    public bool isGettingHitInThisHit;

    private bool isRageMode = false;
    private bool canAttack = true;
    private Transform _playerTransform;
    private NavMeshAgent _navAgent;
    private float currentHealth;
    private float stageDuration = 0f;


    void Start()
    {
        currentHealth = bossHealth.BossMaxHealth;
        _navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HP = currentHealth;
        stageDuration += Time.deltaTime;
        if (player == null)
        {
            Debug.LogWarning("Player object not found.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= attackRange && canAttack)
            {
                if (!isRageMode)
                    Attack();
                else
                    RageAttack();
            }
            else
            {
                if (distanceToPlayer <= retreatRange)
                {
                    Retreat();
                }
                else
                {
                    ChasePlayer();
                }
            }
        }

        if (stageDuration >= 20f && !isRageMode)
        {
            EnterRageMode();
        }
    }

    private void ChasePlayer()
    {
        Vector3 moveDirection = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            // _navAgent.SetDestination(player.transform.position);
            transform.position += moveDirection * chaseSpeed * Time.deltaTime;
            transform.LookAt(player);
        }
    }

    private void Retreat()
    {
        Vector3 retreatDirection = (transform.position - player.position).normalized;
        retreatDirection.y = 0f;
        float retreatSpeed = chaseSpeed * 0.2f;
        Vector3 retreatPosition = transform.position + retreatDirection * retreatSpeed * Time.deltaTime;
        if (!Physics.Raycast(transform.position, retreatDirection, 1f))
        {
            transform.position = retreatPosition;
        }
        transform.LookAt(player);
    }

    private void Attack()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        SpawnHitbox(directionToPlayer);
        StartCoroutine(AttackCooldown());
    }

    private void RageAttack()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        SpawnRageHitbox(directionToPlayer);
        StartCoroutine(AttackCooldown());
    }

    private void SpawnHitbox(Vector3 direction)
    {
        foreach (Transform spawnPoint in hitboxSpawnPoints)
        {
            if (spawnPoint != null)
            {
                GameObject hitbox = Instantiate(hitboxPrefab, spawnPoint.position, Quaternion.identity);
                hitbox.GetComponent<Rigidbody>().velocity = spawnPoint.forward * hitboxSpeed;
                Destroy(hitbox, hitboxDuration);
            }
            else
            {
                Debug.LogError("Spawn points is null.");
            }
        }
    }

    private void SpawnRageHitbox(Vector3 direction)
    {
        Vector3 spawnPosition = attackSpawnPoint.position;
        GameObject rageHitbox = Instantiate(rageHitboxPrefab, spawnPosition, Quaternion.identity);
        rageHitbox.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(rageHitbox, hitboxDuration);
        Destroy(rageHitbox, 1f);
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void EnterRageMode()
    {
        Debug.Log("Boss enters rage mode!");
        isRageMode = true;
        Invoke("ExitRageMode", rageDuration);
    }

    private void ExitRageMode()
    {
        Debug.Log("Boss exits rage mode!");
        isRageMode = false;
        ResetStageDuration();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= bossHealth.BossMaxHealth * 0.5f && !isRageMode)
        {
            EnterRageMode(); 
        }
    }

    private void ResetStageDuration()
    {
        stageDuration = 0f;
    }

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
        isGettingHitInThisHit = false;
    }
}