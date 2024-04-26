using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PatternPatih : MonoBehaviour
{
    public Transform player;
    public int damage, HP, MaxHP;

    public float moveSpeed = 3f;
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
        _playerHealth = GetComponent<Health>();
        _navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > triggerRange && CanMove && !IsStunned)
            {
                Vector3 direction = player.position - transform.position;
                direction.y = 0f;
                direction.Normalize();
                Vector3 movement = direction * moveSpeed * Time.deltaTime;
                transform.position += movement;
            }
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
        }
    }

    public void Attack()
    {
        if ((float)HP > 0.5f * MaxHP)
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
            hitboxInstance.transform.localScale = new Vector3(1f, 1f, hitboxLength);

            Renderer hitboxRenderer = hitboxInstance.GetComponent<Renderer>();
            if (hitboxRenderer != null)
            {
                hitboxRenderer.material.color = Color.red;
            }

            CanMove = false;

            StartCoroutine(StunEnemy());
            StartCoroutine(EnableMovementAfterCast());
        }
        
        if ((float)HP <= 0.5f * MaxHP)
        {
            Renderer enemyRenderer = GetComponent<Renderer>();
            enemyRenderer.material = rageMaterial;
            Vector3 attackDirection = player.position - transform.position;
            attackDirection.y = 0f;
            attackDirection.Normalize();

            // Hitung rotasi hitbox agar menghadap ke arah pemain
            Quaternion hitboxRotation = Quaternion.LookRotation(attackDirection);

            // Tentukan titik awal hitbox di bagian depan musuh (wajah musuh)
            Vector3 startPoint = transform.position + transform.forward * 1f; // Misalnya, jarak hitbox dimulai dari 1 unit di depan musuh

            // Tentukan titik akhir hitbox yang berjarak 7 unit dari startPoint ke arah pemain
            Vector3 endPoint = startPoint + attackDirection * 7f;

            // Buat instance dari hitbox area serangan
            hitboxInstance = Instantiate(hitboxPrefab, startPoint, hitboxRotation);

            // Tentukan panjang hitbox berdasarkan jarak antara startPoint dan endPoint
            float hitboxLength = Vector3.Distance(startPoint, endPoint);

            // Atur skala hitbox agar sesuai dengan panjang yang dihitung
            hitboxInstance.transform.localScale = new Vector3(3f, 1f, hitboxLength);

            Renderer hitboxRenderer = hitboxInstance.GetComponent<Renderer>();
            if (hitboxRenderer != null)
            {
                hitboxRenderer.material.color = Color.red;
            }

            CanMove = false;

            StartCoroutine(StunEnemy());
            StartCoroutine(EnableMovementAfterCast());
        } 
    }

    IEnumerator EnableMovementAfterCast()
    {
        CastDuration = 3f;
        yield return new WaitForSeconds(2f);
        CanMove = true;
    }


    IEnumerator StunEnemy()
    {
        if ((float)HP > 0.5f * MaxHP)
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

            if (enemyRenderer != null && normalMaterial != null)
            {
                enemyRenderer.material = normalMaterial;
            }

            IsStunned = false;
            CanMove = true;
        }
        if((float)HP <= 0.5f * MaxHP)
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

            if (enemyRenderer != null && rageMaterial != null)
            {
                enemyRenderer.material = rageMaterial;
            }

            IsStunned = false;
            CanMove = true;
        }
    }
}
