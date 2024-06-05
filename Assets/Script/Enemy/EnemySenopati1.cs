using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

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
    public bool isGettingHitInThisHit = false;
    public float HP;

    private string playerTag = "Player";
    private Transform player;
    private bool isRageMode = false;
    private bool isAttacking = false;
    private bool hasSpawnedHitboxes = false;
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
            if (isRageMode && !isAttacking)
            {
                RageAttack();
            }
            else if (!isRageMode && !isAttacking)
            {
                NormalAttack();
            }
        }
        else if (distanceToPlayer > attackRange && !isAttacking)
        {
            animator.ResetTrigger("Attack");
            _navAgent.isStopped = false;
            ChasePlayer();
        }

        if (stageDuration >= 20f && !isRageMode)
        {
            EnterRageMode();
        }
    }

    private void ChasePlayer()
    {
        _navAgent.SetDestination(player.position);
        transform.LookAt(player);
    }

    private void NormalAttack()
    {
        transform.LookAt(player);
        _navAgent.isStopped = true;
        hasSpawnedHitboxes = false;
        animator.SetTrigger("Attack");
        StartCoroutine(NormalAnimation());
    }

    private IEnumerator NormalAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(3f);
        
        if (!hasSpawnedHitboxes)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            SpawnHitbox(directionToPlayer);

            Vector3 spawnPosition = attackSpawnPoint.position;
            GameObject areaHitbox = Instantiate(areaAttackHitbox, spawnPosition, Quaternion.identity);
            Destroy(areaHitbox, hitboxDuration);

            hasSpawnedHitboxes = true;
        }
        
        yield return new WaitForSeconds(2f);
        isAttacking = false;
    }

    private void RageAttack()
    {
        transform.LookAt(player);
        _navAgent.isStopped = true;
        hasSpawnedHitboxes = false;
        animator.SetTrigger("Attack");
        StartCoroutine(RageAnimation());
    }

    private IEnumerator RageAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(3f);
        
        if (!hasSpawnedHitboxes)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            SpawnRageHitbox(directionToPlayer);

            Vector3 spawnPosition = attackSpawnPoint.position;
            GameObject areaHitbox = Instantiate(areaAttackHitbox, spawnPosition, Quaternion.identity);
            Destroy(areaHitbox, hitboxDuration);

            hasSpawnedHitboxes = true;
        }
        
        yield return new WaitForSeconds(2f);
        isAttacking = false;
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
