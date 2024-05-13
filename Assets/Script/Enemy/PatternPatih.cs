using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PatternPatih : MonoBehaviour
{
    // public Transform player;
    public int damage, HP, MaxHP;

    public float moveSpeed = 5f;
    public float attackRange = 7f;
    public float triggerRange = 4f;
    public float CastDuration = 3f;
    public float StunDuration = 3f;

    public bool Charging = false;
    public bool CanMove = true;
    public bool IsStunned = false;
    public bool IsRage = false;

    public GameObject hitboxPrefab;
    public Material normalMaterial;
    public Material stunMaterial;
    public Material rageMaterial;
    public BossHealth bossHealth;

    private string playerTag = "Player";
    private Transform player;
    private NavMeshAgent _navAgent;
    private Health _playerHealth;
    private GameObject hitboxInstance;
    public bool IsGettingHitInThisHit = false;

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
        HP = bossHealth.BossMaxHealth;
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        _playerHealth = GetComponent<Health>();
        _navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navAgent.speed = moveSpeed;
        if (IsRage)
        {
            Renderer enemyRenderer = GetComponent<Renderer>();
            enemyRenderer.material = rageMaterial;
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);

            ChasingPlayer();
        }
    }

    public void ChasingPlayer()
    {
        if (HP <= 0.5 * MaxHP)
            {
                IsRage = true;
            }

        if (player != null && _navAgent != null)
        {
            if (IsStunned && !Charging)
            {
                _navAgent.isStopped = true;
                return;
            }
            else
            {
                _navAgent.isStopped = false;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= triggerRange && CanMove && !IsStunned)
            {
                Charging = true;
                CastDuration -= Time.deltaTime;
                if (CastDuration <= 0f)
                {
                    Attack();
                    Charging = false;
                }
            }

            _navAgent.SetDestination(player.position);
            _navAgent.stoppingDistance = triggerRange;
        }
    }

    public void Attack()
    {
        Vector3 attackDirection = player.position - transform.position;
        attackDirection.y = 0f;
        attackDirection.Normalize();

        // Hitung rotasi hitbox agar menghadap ke arah pemain
        Quaternion hitboxRotation = Quaternion.LookRotation(attackDirection);

        // Tentukan titik awal hitbox di bagian depan musuh (wajah musuh)
        Vector3 startPoint = transform.position + transform.forward * 1f;

        // Tentukan titik akhir hitbox yang berjarak 7 unit dari startPoint ke arah pemain
        Vector3 endPoint = startPoint + attackDirection * 7f;

        // Buat instance dari hitbox area serangan
        hitboxInstance = Instantiate(hitboxPrefab, startPoint, hitboxRotation);

        // Tentukan panjang hitbox berdasarkan jarak antara startPoint dan endPoint
        float hitboxLength = Vector3.Distance(startPoint, endPoint);

        // Atur skala hitbox agar sesuai dengan panjang yang dihitung
        if (!IsRage)
        {
            hitboxInstance.transform.localScale = new Vector3(1f, 1f, hitboxLength);
        }
        if (IsRage)
        {
            hitboxInstance.transform.localScale = new Vector3(3f, 1f, hitboxLength);
        }
           
        Renderer hitboxRenderer = hitboxInstance.GetComponent<Renderer>();
        if (hitboxRenderer != null)
        {
            hitboxRenderer.material.color = Color.red;
        }

        CanMove = false;

        StartCoroutine(StunEnemy());
        StartCoroutine(EnableMovementAfterCast());
    }

    IEnumerator EnableMovementAfterCast()
    {
        CastDuration = 3f;
        yield return new WaitForSeconds(2f);
        CanMove = true;
    }


    IEnumerator StunEnemy()
    {
        yield return new WaitForSeconds(0.5f);

        if (hitboxInstance != null)
        {
            Destroy(hitboxInstance);
        }

        IsStunned = true;

        Renderer enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null && stunMaterial != null)
        {
            enemyRenderer.material = stunMaterial;
        }

        if (_playerHealth != null)
        {
            _playerHealth.TakeDamage(damage);
        }

        while (StunDuration > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            StunDuration -= Time.deltaTime;
        }

        StunDuration = 3f;

        if (!IsRage)
        {
            enemyRenderer.material = normalMaterial;
        }
        if (IsRage)
        {
            enemyRenderer.material = rageMaterial;
        }

        IsStunned = false;
        CanMove = true;
    }
}