using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Senopati : MonoBehaviour
{
    public Transform[] hitboxSpawnPoints;
    public GameObject hitboxPrefab;
    public GameObject areaAttackHitbox;
    public GameObject rageHitboxPrefab;
    public Transform attackSpawnPoint;
    public BossHealth bossHealth;

    public float attackRange = 6f;
    public float hitboxSpeed = 10f;
    public float hitboxDuration = 1f;
    public float attackCooldown = 4f;
    public bool isGettingHitInThisHit = false;
    public float HP;

    private string playerTag = "Player";
    private Transform player;
    private bool isRageMode = false;
    private bool canAttack = true;
    private NavMeshAgent _navAgent;
    private float currentHealth;
    private float stageDuration = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = bossHealth.BossMaxHealth;
        _navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        Debug.Log("Senopati HP: " + currentHealth);
    }

    void Update()
    {
        stageDuration += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            _navAgent.isStopped = true;
            if (!isRageMode && canAttack)
            {
                Attack();
            }
            else if (isRageMode && canAttack)
            {
                RageAttack();
            }
        }
        else
        {
            _navAgent.isStopped = false;
            animator.SetBool("Attack Zone", false);
            ChasePlayer();
        }

        if (stageDuration >= 20f && !isRageMode)
        {
            EnterRageMode();
        }

        if (isRageMode && !canAttack)
        {
            _navAgent.isStopped = true;
        }
    }

    private void ChasePlayer()
    {
        _navAgent.SetDestination(player.position);
    }

    private void Attack()
    {
        animator.SetBool("Attack Zone", true);
        StartCoroutine(AnimationToAttack());
    }
    private IEnumerator AnimationToAttack()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        SpawnHitbox(directionToPlayer);
        Vector3 spawnPosition = attackSpawnPoint.position;
        GameObject areaHitbox = Instantiate(areaAttackHitbox, spawnPosition, Quaternion.identity);
        Destroy(areaHitbox, hitboxDuration);
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
                Debug.LogError("Spawn point is null.");
            }
        }
    }

    private void SpawnRageHitbox(Vector3 direction)
    {
        Vector3 spawnPosition = attackSpawnPoint.position;
        GameObject rageHitbox = Instantiate(rageHitboxPrefab, spawnPosition, Quaternion.identity);
        rageHitbox.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(rageHitbox, hitboxDuration);
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void EnterRageMode()
    {
        isRageMode = true;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Senopati Damaged");
        currentHealth -= damage;
        Debug.Log("Senopati HP: " + currentHealth);
        if (currentHealth <= bossHealth.BossMaxHealth * 0.5f && !isRageMode)
        {
            EnterRageMode();
        }
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
