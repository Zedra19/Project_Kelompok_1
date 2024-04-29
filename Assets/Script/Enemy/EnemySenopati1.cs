using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float attackRange = 3f;
    public float chaseSpeed = 5f;
    public GameObject hitboxPrefab;
    public GameObject rageHitboxPrefab;
    public float hitboxSpeed = 10f;
    public float hitboxDuration = 1f;
    public float rageThreshold = 0.5f;
    public float rageDuration = 20f;
    public float attackCooldown = 4f;
    public float maxHealth = 100f;
    public Transform attackSpawnPoint; // Objek Transform untuk titik spawn serangan
    // public Transform attackSpawnPointMide;
    // public Transform attackSpawnPointLeft;
    // public Transform attackSpawnPointRight;
    public Transform[] hitboxSpawnPoints;

    private bool isRageMode = false;
    private bool canAttack = true;
    private float currentHealth;
    private float stageDuration = 0f;
   

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        stageDuration += Time.deltaTime;
        if (player == null)
        {
            Debug.LogWarning("Player object not found.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer < attackRange) // Ubah nilai 1.5f sesuai dengan jarak minimum yang diinginkan
            {
                Retreat();
                 if (distanceToPlayer == attackRange && canAttack)
                {
                    if (!isRageMode)
                        Attack();
                    else
                        RageAttack();
                }
            }
            else
            {
                    ChasePlayer();
            }
        }

        if (stageDuration >= 20f && !isRageMode)
        {
            EnterRageMode();
        }
        
        // if (distanceToPlayer <= attackRange && canAttack)
        // {
        //     // Jarak terlalu dekat, boss mundur untuk menjaga jarak
        //     if (distanceToPlayer <= 1.5f) // Ubah nilai 1.5f sesuai dengan jarak minimum yang diinginkan
        //     {
        //         Retreat();
        //     }
        // }
    }

    void ChasePlayer()
    {
        Vector3 moveDirection = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            transform.position += moveDirection * chaseSpeed * Time.deltaTime;
            transform.LookAt(player);
        }
    }

    void Attack()
    {
        StopChasing();

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        SpawnHitbox(directionToPlayer);
        StartCoroutine(AttackCooldown());
        StartCoroutine(ResumeChasingAfterDelay(2f));
    }

    void RageAttack()
    {
        StopChasing();

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        SpawnRageHitbox(directionToPlayer);
        StartCoroutine(AttackCooldown());
        StartCoroutine(ResumeChasingAfterDelay(5f));
    }

    void StopChasing()
    {
        
    }

    IEnumerator ResumeChasingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Implement your logic to resume chasing here
    }

    void SpawnHitbox(Vector3 direction)
    {
        foreach (Transform spawnPoint in hitboxSpawnPoints)
        {
            // Pastikan spawnPoint tidak null
            if (spawnPoint != null)
            {
                // Munculkan hitbox di titik spawn yang sedang diproses dan arahkan ke depan
                GameObject hitbox = Instantiate(hitboxPrefab, spawnPoint.position, Quaternion.identity);
                hitbox.GetComponent<Rigidbody>().velocity = spawnPoint.forward * hitboxSpeed;
                Destroy(hitbox, hitboxDuration);
            }
            else
            {
                Debug.LogError("One of the hitbox spawn points is null.");
            }
        }
        // Vector3 spawnPosition = attackSpawnPoin.position; // Menggunakan posisi dari objek transform sebagai titik spawn
        // GameObject hitbox = Instantiate(hitboxPrefab, spawnPosition, Quaternion.identity);
        // hitbox.GetComponent<Rigidbody>().velocity = direction.normalized * hitboxSpeed;
        // Destroy(hitbox, hitboxDuration);
    }

    void SpawnRageHitbox(Vector3 direction)
    {
        // foreach (Transform spawnPoint in hitboxSpawnPoints)
        // {
        //     // Pastikan spawnPoint tidak null
        //     if (spawnPoint != null)
        //     {
        //         // Munculkan hitbox di titik spawn yang sedang diproses dan arahkan ke depan
        //         GameObject rageHitbox = Instantiate(rageHitboxPrefab, spawnPoint.position, Quaternion.identity);
        //         rageHitbox.GetComponent<Rigidbody>().velocity = spawnPoint.forward * hitboxSpeed;
        //         Destroy(rageHitbox, hitboxDuration);
        //     }
        //     else
        //     {
        //         Debug.LogError("One of the hitbox spawn points is null.");
        //     }
        // }
        Vector3 spawnPosition = attackSpawnPoint.position; // Menggunakan posisi dari objek transform sebagai titik spawn
        GameObject rageHitbox = Instantiate(rageHitboxPrefab, spawnPosition, Quaternion.identity);
        rageHitbox.GetComponent<Rigidbody>().velocity = Vector3.zero; // Set kecepatan hitbox menjadi nol
        Destroy(rageHitbox, hitboxDuration); // Hancurkan hitbox setelah durasi yang ditentukan
        Destroy(rageHitbox, 1f); // Hancurkan hitbox setelah 1 detik
    }
    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnTriggerEnter(Collider other)
    {
    }

    void EnterRageMode()
    {
        Debug.Log("Boss enters rage mode!");
        isRageMode = true;
        Invoke("ExitRageMode", rageDuration);
    }

    void ExitRageMode()
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
        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= maxHealth * rageThreshold && !isRageMode)
        {
            EnterRageMode(); // Panggil EnterRageMode ketika kesehatan bos mencapai batas kemarahan
        }
    }
    void Retreat()
    {
        // Hitung vektor arah mundur dari boss ke player
        Vector3 retreatDirection = (transform.position - player.position).normalized;
        // Menghindari gerakan vertikal
        retreatDirection.y = 0f;
        // Tentukan posisi mundur untuk boss
        Vector3 retreatPosition = transform.position + retreatDirection * chaseSpeed * Time.deltaTime;
        // Hindari benturan dengan objek lain
        if (!Physics.Raycast(transform.position, retreatDirection, 1f))
        {
            // Terapkan pergerakan mundur
            transform.position = retreatPosition;
        }
    }

    void ResetStageDuration()
    {
        stageDuration = 0f;
    }
    void Die()
    {
        Debug.Log("Boss died!");
        Destroy(gameObject);
    }
}
