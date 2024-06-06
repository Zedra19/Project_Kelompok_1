using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using System;

public class Senopati : MonoBehaviour
{

    public static event Action OnSenopatiDestroyed;

    // Public variables
    public Transform[] hitboxSpawnPoints;
    public GameObject hitboxPrefab;
    public GameObject areaAttackHitbox;
    public GameObject rageHitboxPrefab;
    public Transform attackSpawnPoint;
    public BossHealth bossHealth;

    // Parameters
    public float attackRange = 6f;
    public float hitboxSpeed = 10f;
    public float hitboxDuration = 1f;
    public bool isGettingHitInThisHit = false;
    public float HP;

    // Private variables
    private string playerTag = "Player";
    private Transform player;
    private bool isRageMode = false;
    private bool isAttacking = false;
    private bool hasSpawnedHitboxes = false;
    private NavMeshAgent _navAgent;
    private float currentHealth;
    private float stageDuration = 0f;
    private Animator animator;

    // Called when the script instance is being loaded
    void Start()
    {
        // Initialize components and variables
        animator = GetComponent<Animator>();
        currentHealth = bossHealth.BossMaxHealth;
        _navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        Debug.Log("Senopati HP: " + currentHealth);
    }

    // Called once per frame
    void Update()
    {
        stageDuration += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Handle attack and chase logic
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

        // Enter rage mode after a certain duration
        if (stageDuration >= 20f && !isRageMode)
        {
            EnterRageMode();
        }
    }

    // Chase the player
    private void ChasePlayer()
    {
        _navAgent.SetDestination(player.position);
        transform.LookAt(player);
    }

    // Perform normal attack
    private void NormalAttack()
    {
        transform.LookAt(player);
        _navAgent.isStopped = true;
        hasSpawnedHitboxes = false;
        animator.SetTrigger("Attack");
        StartCoroutine(NormalAnimation());
    }

    // Coroutine for normal attack animation and hitbox spawning
    private IEnumerator NormalAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(2.7f);

        if (!hasSpawnedHitboxes)
        {
            // Spawn hitboxes for normal attack
            SpawnProjectil();
            SpawnHitbox();

            hasSpawnedHitboxes = true;
        }

        yield return new WaitForSeconds(2f);
        isAttacking = false;
    }

    // Perform rage attack
    private void RageAttack()
    {
        transform.LookAt(player);
        _navAgent.isStopped = true;
        hasSpawnedHitboxes = false;
        animator.SetTrigger("Attack");
        StartCoroutine(RageAnimation());
    }

    // Coroutine for rage attack animation and hitbox spawning
    private IEnumerator RageAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(2.7f);

        if (!hasSpawnedHitboxes)
        {
            // Spawn hitbox for rage attack
            SpawnRageHitbox();

            hasSpawnedHitboxes = true;
        }

        yield return new WaitForSeconds(2f);
        isAttacking = false;
    }

    // Spawn projectile hitbox
    private void SpawnProjectil()
    {
        foreach (Transform spawnPoint in hitboxSpawnPoints)
        {
            GameObject hitbox = Instantiate(hitboxPrefab, spawnPoint.position, Quaternion.LookRotation(spawnPoint.forward));
            hitbox.GetComponent<Rigidbody>().velocity = spawnPoint.forward * hitboxSpeed;
            Destroy(hitbox, hitboxDuration);
        }
    }

    // Spawn area hitbox
    private void SpawnHitbox()
    {
        AudioManager.Instance.PlaySFXForDuration("SenopatiQuake", 1);
        Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        Vector3 spawnPosition = attackSpawnPoint.position;
        GameObject areaHitbox = Instantiate(areaAttackHitbox, spawnPosition, rotation);
        Destroy(areaHitbox, hitboxDuration);
    }

    // Spawn rage hitbox
    private void SpawnRageHitbox()
    {
        AudioManager.Instance.PlaySFXForDuration("SenopatiQuake", 1);
        Vector3 spawnPosition = attackSpawnPoint.position;
        Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        GameObject rageHitbox = Instantiate(rageHitboxPrefab, spawnPosition, rotation);
        Destroy(rageHitbox, hitboxDuration);
    }

    // Enter rage mode
    private void EnterRageMode()
    {
        isRageMode = true;
    }

    // Get current health of the boss
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Apply damage to the boss
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

    // Enable event listener
    private void OnEnable()
    {
        PlayerAttack.OnAttackDone += AllowAttack;
    }

    // Disable event listener
    private void OnDisable()
    {
        PlayerAttack.OnAttackDone -= AllowAttack;
    }

    // Allow the boss to attack again
    private void AllowAttack()
    {
        isGettingHitInThisHit = false;
    }

    private void OnDestroy()
    {
        OnSenopatiDestroyed?.Invoke();
    }
}
